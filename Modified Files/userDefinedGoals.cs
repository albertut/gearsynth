using System;
using System.Collections.Generic;
using System.Text;

namespace GraphSynth
{
    public class userDefinedGoals
    {
        public double[,] inputLocation = new double[4, 4];
        public double[,] outputLocation = new double[4, 4];

        public double inputSpeed = 200;
        public double outputSpeed = 800;
        public double inputTorque = 1.0; //ft lbs
        public double outputTorque = 0;
        public double maxX = 10;
        public double minX = -10;
        public double maxY = 10;
        public double minY = -10;
        public double maxZ = 10;
        public double minZ = -10;
        public userDefinedGoals()
        {
            inputLocation = Matrix.makeIdentity(4);
            outputLocation = Matrix.multiply(inputLocation, Matrix.translate(2.0, 2.0, 4.0));
            outputLocation = Matrix.multiply(outputLocation, Matrix.rotationX(-40));
            //outputLocation = Matrix.multiply(outputLocation, Matrix.rotationY(20));
        }
    }
}