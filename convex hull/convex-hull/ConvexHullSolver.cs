using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;

namespace _2_convex_hull
{
    class Line{
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
            SortByXCoordinate(pointList);
            List<System.Drawing.PointF> finalHull = SolvePoints(pointList);

        }

        public List<System.Drawing.PointF> SolvePoints(List<System.Drawing.PointF> pointList){
            
            int size = pointList.Count;
            if (size == 1){
                return pointList;
            }

            int half_size_ceiling = (size + 1 / 2);
            List<System.Drawing.PointF> left_hull = SolvePoints(pointList.GetRange(0, half_size_ceiling - 1));
            List<System.Drawing.PointF> right_hull = SolvePoints(pointList.GetRange(half_size_ceiling, size - half_size_ceiling));
            return CombineHulls(left_hull, right_hull);
        }

        List<System.Drawing.PointF> CombineHulls(List<System.Drawing.PointF> left_hull
                                                 List<System.Drawing.PointF> righit_hull){

            return left_hull;
        }

        void SortByXCoordinate(List<System.Drawing.PointF> pointList){
            //this.quickSort(pointList);
            //TODO: Implement this function
        }

        void quickSort(List<System.Drawing.PointF> pointList){
            PointF pivot = (PointF)pointList[0];
            for (int i = 0; i < pointList.Count; i++){
                for (int j = 0; j < pointList.Count; j++){
                    
                }
            }
        }
    }
}
