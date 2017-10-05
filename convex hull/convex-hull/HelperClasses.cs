using System;
using System.Collections.Generic;
using System.Drawing;

namespace _1_convex_hull
{

	public class ConvexHull
	{

		private List<System.Drawing.PointF> points;
		private PointF rightmost_point;
		private PointF leftmost_point;

        public ConvexHull(){
			this.rightmost_point = new PointF();
			this.leftmost_point = new PointF();
            this.points = new List<PointF>();
		}

        public ConvexHull(List<PointF> points){
            this.rightmost_point = new PointF();
            this.leftmost_point = new PointF();
            this.points = points;
        }

		public ConvexHull(List<PointF> points, PointF right_point, PointF left_point)
		{
			this.points = points;
			this.leftmost_point = left_point;
			this.rightmost_point = right_point;
		}

        public int PointCount(){
            return points.Count;
        }

        public ConvexHull LeftHalf()
        {
            int half_size_ceiling = ((this.PointCount() + 1) / 2);
            List<PointF> left_points = points.GetRange(0, half_size_ceiling);
            return new ConvexHull(left_points);
        }

        public ConvexHull RightHalf()
        {
			int half_size_ceiling = ((this.PointCount() + 1) / 2);
            List<PointF> right_points = points.GetRange(half_size_ceiling, this.PointCount() - half_size_ceiling);
            return new ConvexHull(right_points);
		}

        public bool isOrdered(){
            if (!rightmost_point.IsEmpty && !leftmost_point.IsEmpty){
                return true;
            } else {
                return false;
            }
        }

		public List<PointF> GetPoints()
		{
			return points;
		}

		public PointF GetRightmostPoint()
		{
			return rightmost_point;
		}

		public PointF GetLeftmostPoint()
		{
			return leftmost_point;
		}

        public void SetRightmostPoint(PointF p){
            rightmost_point = p;
        }

        public void SetLeftmostPoint(PointF p){
            leftmost_point = p;
        }

        public PointF NextCounterClockwisePoint(PointF referencePoint){

			if (points.Count == 0) {
				throw new Exception();
			}

			if (points.Count == 1) {
				return points[0];
			}

            int refIndex = points.IndexOf(referencePoint);
            if ((refIndex - 1) == -1){
                return points[points.Count - 1];
            } else {
                return points[refIndex - 1];
            }
        }

        public PointF NextClockwisePoint(PointF referencePoint){

            if(points.Count == 0){
                throw new Exception();
            }

            if(points.Count == 1){
                return points[0];
            }

            int refIndex = points.IndexOf(referencePoint);
            if ((refIndex + 1) == points.Count){
                return points[0];
            } else {
                return points[refIndex + 1];
            }
        }

	}

    enum Direction
    {
        Left, Right
    };

	public class TangentLine
	{
        private PointF left_point;
        private PointF right_point;

        public TangentLine(){
            this.left_point = new PointF();
            this.right_point = new PointF();
        }

        /* Even if the user messes up, the points will be assigned correctly */
        public TangentLine(PointF pLeft, PointF pRight){

            PointF left_p = pLeft.X < pRight.X ? pLeft : pRight;
            PointF right_p = pRight.X > pLeft.X ? pRight : pLeft;

            this.left_point = left_p;
            this.right_point = right_p;
        }

        public void SetLeftPoint(PointF newP){
            left_point = newP;
        }

        public void SetRightPoint(PointF newP){
            right_point = newP;
        }

        public PointF GetLeftPoint(){
            return left_point.X < right_point.X ? left_point : right_point;
        }

        public PointF GetRightPoint(){
            return right_point.X > left_point.X ? right_point : left_point;
        }

        public bool IsUpperTangentTo(ConvexHull left_hull, ConvexHull right_hull){
            foreach (PointF point in left_hull.GetPoints()){
                double tangentY = GetTangentSlope() * point.X + YIntercept(); // Tangent Y value at Point's X
				if (point.Y <= tangentY){
                    return false;
                }
            }

            foreach (PointF point in right_hull.GetPoints()){
                double tangentY = GetTangentSlope() * point.X + YIntercept(); // Tangent Y value at Point's X
				if (point.Y <= tangentY){
                    return false;
                }
            }
            return true;
        }

        public bool IsLowerTangentTo(ConvexHull left_hull, ConvexHull right_hull){
            foreach (PointF point in left_hull.GetPoints()){
                double tangentY = GetTangentSlope() * point.X + YIntercept(); // Tangent Y value at Point's X
                if (point.Y >= tangentY){
                    return false;
                }
            }

            foreach (PointF point in right_hull.GetPoints()){
                double tangentY = GetTangentSlope() * point.X + YIntercept(); // Tangent Y value at Point's X
				if (point.Y >= tangentY){
                    return false;
                }
            }
            return true;
        }

        public double GetTangentSlope(){
            return (left_point.Y - right_point.Y) / (left_point.X - right_point.X);
        }

        public double YIntercept(){
            double intercept = left_point.Y - GetTangentSlope() * left_point.X;
            return intercept;
        }
	}

}
