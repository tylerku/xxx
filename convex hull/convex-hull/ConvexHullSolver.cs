using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Linq;
using _1_convex_hull;

namespace _2_convex_hull
{

    class Line {
        PointF p1;
        PointF p2;
    }

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

        }

        public ConvexHull SolvePoints(ConvexHull hull){

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
            TangentLine upper_tangent = new TangentLine();
            double old_slope = GetSlopeFromPoints(tangent_left_point, tangent_right_point);
                             
            while (!upper_tangent.IsUpperTangentTo(left_hull, right_hull))
            {

				bool temp_upper_left_point_found = false;
				bool temp_upper_right_point_found = false;

                while (temp_upper_left_point_found == false)
                {
                    PointF next_point = left_hull.NextCounterClockwisePoint(tangent_left_point);
                    double new_slope = GetSlopeFromPoints(next_point, tangent_right_point);
                    if(new_slope >= old_slope){
                        tangent_left_point = left_hull.NextCounterClockwisePoint(tangent_left_point);
                        old_slope = new_slope;
                        continue;
                    } else {
                        temp_upper_left_point_found = true;
                    }

                }

                while (upper_right_point_found == false)
                {
                    //TODO   
                }
            }

			return new TangentLine(tangent_left_point, tangent_right_point);

		}

        TangentLine FindLowerTangent(ConvexHull left_hull, ConvexHull right_hull)
        {
			PointF left_point;
			PointF right_point;
			bool lower_left_point_found = false;
			bool lower_right_point_found = false;
			TangentLine lower_tangent = new TangentLine();


			/* Find the left and right points if either of the hulls.Count is < 3 */

			if (left_hull.PointCount() == 1) {
				left_point = left_hull.GetPoints()[0];
				lower_left_point_found = true;
			}
			
			if (right_hull.PointCount() == 1) {
				right_point = right_hull.GetPoints()[0];
				lower_right_point_found = true;
			}

			/* If both left and right are set, then we are done */

			if (lower_left_point_found && lower_right_point_found)
			{
				lower_tangent.setLeftPoint(left_point);
				lower_tangent.setRightPoint(right_point);
				return lower_tangent;
			}

			/* Get the initial values of our leftmost and rightmost points */

			if (left_point.IsEmpty)
			{
				left_point = left_hull.GetRightmostPoint();
			}
			if (right_point.IsEmpty)
			{
				right_point = right_hull.GetLeftmostPoint();
			}

            while(!lower_tangent.IsLowerTangentTo(left_hull, right_hull)){
				lower_left_point_found = false;
				lower_right_point_found = false;

				while (lower_left_point_found == false)
				{
                    //TODO
                }

				while (lower_right_point_found == false)
				{
					//TODO   
				}
            }

            return lower_tangent;

			


			/* Find the left and right lower points if either of the hulls.Count is < 3 */


		}

        double GetSlopeFromPoints(PointF p1, PointF p2){
            return (p1.Y - p2.Y) / (p1.X - p2.X);
        }
    }
}
