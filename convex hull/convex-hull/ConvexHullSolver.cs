using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Linq;
using _1_convex_hull;

namespace _2_convex_hull
{

    class ConvexHullSolver
    {

        System.Drawing.Graphics g;
        System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

		public void Solve(List<System.Drawing.PointF> pointList)
        {
            //SortByXCoordinate(pointList);
            List<PointF> sortedList = pointList.OrderBy(p => p.X).ToList();
            ConvexHull finalHull = SolvePoints(new ConvexHull(sortedList));
			DrawConvexHull(finalHull);

		}

        ConvexHull SolvePoints(ConvexHull hull){

            if (hull.PointCount() == 1){
                hull.SetLeftmostPoint(hull.GetPoints()[0]);
                hull.SetRightmostPoint(hull.GetPoints()[0]);
                return hull;
            }

            ConvexHull left_hull = SolvePoints(hull.LeftHalf());
            ConvexHull right_hull = SolvePoints(hull.RightHalf());
            return CombineHulls(left_hull, right_hull);
        }

        ConvexHull CombineHulls(ConvexHull left_hull,
                                                 ConvexHull right_hull){
            
            TangentLine upper_tangent = FindUpperTangent(left_hull, right_hull);
            TangentLine lower_tangent = FindLowerTangent(left_hull, right_hull);
            return CreateNewHull(upper_tangent, lower_tangent, left_hull, right_hull);
        }

        ConvexHull CreateNewHull(TangentLine upper_tangent,
                                                  TangentLine lower_tangent,
                                                  ConvexHull left_hull,
                                                  ConvexHull right_hull){
            List<PointF> newPoints = new List<PointF>();
            PointF current_point = lower_tangent.GetLeftPoint();
            newPoints.Add(current_point);
            current_point = left_hull.NextClockwisePoint(current_point);

            while(!current_point.Equals(upper_tangent.GetLeftPoint())){
                newPoints.Add(current_point);
                current_point = left_hull.NextClockwisePoint(current_point);
            }

            /* Don't want to add the point when left point of lower_tangent ==
             * left point of upper_tangent becuase the point was already added */
            if(!newPoints.Contains(current_point)){
                newPoints.Add(current_point);
            }

            current_point = upper_tangent.GetRightPoint();
            newPoints.Add(current_point);
            current_point = right_hull.NextClockwisePoint(current_point);

            while(!current_point.Equals(lower_tangent.GetRightPoint())){
                newPoints.Add(current_point);
                current_point = right_hull.NextClockwisePoint(current_point);
            }

			/* Don't want to add the point when right point of upper_tangent ==
			 * right point of lower_tangent becuase the point was already added */
			if(!newPoints.Contains(current_point)){
                newPoints.Add(current_point);
            }


            ConvexHull createdHull = new ConvexHull(newPoints, right_hull.GetRightmostPoint(), left_hull.GetLeftmostPoint());
			return createdHull;
        }

        TangentLine FindUpperTangent(ConvexHull left_hull, ConvexHull right_hull){
            PointF tangent_left_point = left_hull.GetRightmostPoint();
            PointF tangent_right_point = right_hull.GetLeftmostPoint();
            TangentLine upper_tangent = new TangentLine(tangent_left_point, tangent_right_point);
            double old_slope = GetSlopeFromPoints(tangent_left_point, tangent_right_point);

            /* 'recently' here refers to "since the while loop has been ran" */
			bool left_point_changed_recently = true;
			bool right_point_changed_recently = true;

            while (left_point_changed_recently || right_point_changed_recently){

				bool temp_upper_left_point_found = false;
				bool temp_upper_right_point_found = false;

                /* reset these variables to see if these points change this iteration */
                left_point_changed_recently = false;
                right_point_changed_recently = false;

                while (temp_upper_left_point_found == false)
                {
                    PointF next_point = left_hull.NextCounterClockwisePoint(tangent_left_point);
                    double new_slope = GetSlopeFromPoints(next_point, tangent_right_point);
                    if(new_slope > old_slope){
                        tangent_left_point = next_point;
						left_point_changed_recently = true;
						old_slope = new_slope;
                        continue;
                    } else {
                        upper_tangent.SetLeftPoint(tangent_left_point);
                        temp_upper_left_point_found = true;
                    }

                }

                while (temp_upper_right_point_found == false)
                {
                    PointF next_point = right_hull.NextClockwisePoint(tangent_right_point);
                    double new_slope = GetSlopeFromPoints(tangent_left_point, next_point);
                    if(new_slope < old_slope){
                        tangent_right_point = next_point;
                        old_slope = new_slope;
                        right_point_changed_recently = true;
                        continue;
                    } else {
                        upper_tangent.SetRightPoint(tangent_right_point);
                        temp_upper_right_point_found = true;
                    }
                }
            }

            return upper_tangent;

		}

        TangentLine FindLowerTangent(ConvexHull left_hull, ConvexHull right_hull)
        {
			PointF tangent_left_point = left_hull.GetRightmostPoint();
			PointF tangent_right_point = right_hull.GetLeftmostPoint();
            TangentLine lower_tangent = new TangentLine(tangent_left_point, tangent_right_point);
            double old_slope = GetSlopeFromPoints(tangent_left_point, tangent_right_point);

			/* 'recently' here refers to "since the while loop has been ran" */
			bool left_point_changed_recently = true;
			bool right_point_changed_recently = true;

			while (left_point_changed_recently || right_point_changed_recently){

				bool temp_lower_left_point_found = false;
				bool temp_lower_right_point_found = false;

				/* reset these variables to see if these points change this iteration */
				left_point_changed_recently = false;
				right_point_changed_recently = false;

				while (temp_lower_left_point_found == false)
				{
                    PointF next_point = left_hull.NextClockwisePoint(tangent_left_point);
                    double new_slope = GetSlopeFromPoints(next_point, tangent_right_point);
                    if (new_slope < old_slope){
                        tangent_left_point = next_point;
                        left_point_changed_recently = true;
                        old_slope = new_slope;
                        continue;
                    } else {
                        lower_tangent.SetLeftPoint(tangent_left_point);
                        temp_lower_left_point_found = true;
                    }
                }

				while (temp_lower_right_point_found == false)
				{
                    PointF next_point = right_hull.NextCounterClockwisePoint(tangent_right_point);
                    double new_slope = GetSlopeFromPoints(tangent_left_point, next_point);
                    if (new_slope > old_slope) {
                        tangent_right_point = next_point;
                        right_point_changed_recently = true;
                        old_slope = new_slope;
                        continue;
                    } else {
                        lower_tangent.SetRightPoint(tangent_right_point);
                        temp_lower_right_point_found = true;
                    }
				}
            }

            return lower_tangent;

		}

        double GetSlopeFromPoints(PointF p1, PointF p2){
            return (p1.Y - p2.Y) / (p1.X - p2.X);
        }

        void DrawConvexHull(ConvexHull hull){
            Graphics gfx = pictureBoxView.CreateGraphics();
            gfx.DrawLines(Pens.Blue, hull.GetPoints().ToArray());
        }
    }
}

        
