using System;
using System.Collections.Generic;
using System.Text;

namespace GraphSynth
{
    class GearFamily
    {
        public string label;
        public string details;
        public double density;

        public double Sfb;
        public double Sfc;

        public double[,] gears;

        internal static List<GearFamily> loadDefaults()
        {            
            List<GearFamily> defaults  = new List<GearFamily>();

            #region plastic gear family, plastic14.5
            GearFamily plastic145 = new GearFamily()
            {
                //label = "plastic14.5",
                label = "typeA",
                details = "a generic class of plastic gears with pa of 14.5",
                Sfb = 5000,
                Sfc = 50000,

                
                density = 0.04,//ib/cu in
                gears = new double[,]       
                //teeth, pitch, facewidth
                {   {11, 20, 0.375},
                    {60, 20, 0.375},
                    {16, 16, 0.5},
                    {24, 16, 0.5},
                    {32, 16, 0.5},
                    {48, 16, 0.5},
                    {14, 12, 0.75},
                    {48, 12, 0.75},
                    {12, 10, 1.0},
                    {20, 10, 1.0}}
            };
            defaults.Add(plastic145);
            #endregion
            #region steel14.5
            GearFamily steel145 = new GearFamily()
            {
                label = "steel14.5",
                details = "a generic class of steel gears with pa 145",
                Sfb = 50000,
                Sfc = 100000,
                

                density = 0.283,//ib/cu in
                gears = new double[,]          
                {   {16, 32, 0.1875},
                    {18, 32, 0.1875},
                    {20, 32, 0.1875},
                    {24, 32, 0.1875},
                    {40, 32, 0.1875},
                    {12, 24, 0.25},
                    {15, 24, 0.25},
                    {16, 24, 0.25},
                    {20, 24, 0.25},
                    {21, 24, 0.25},
                    {24, 24, 0.25},
                    {36, 24, 0.25},
                    {48, 24, 0.25},
                    {72, 24, 0.25},
                    {12, 20, 0.375},
                    {15, 20, 0.375},
                    {16, 20, 0.375},
                    {20, 20, 0.375},
                    {23, 20, 0.375},
                    {24, 20, 0.375},
                    {25, 20, 0.375},
                    {32, 20, 0.375},
                    {36, 20, 0.375},
                    {48, 20, 0.375},
                    {60, 20, 0.375},
                    {12, 16, 0.5},
                    {14, 16, 0.5},
                    {16, 16, 0.5},
                    {18, 16, 0.5},
                    {20, 16, 0.5},
                    {22, 16, 0.5},
                    {24, 16, 0.5},
                    {28, 16, 0.5},
                    {30, 16, 0.5},
                    {32, 16, 0.5},
                    {36, 16, 0.5},
                    {40, 16, 0.5},
                    {48, 16, 0.5},
                    {60, 16, 0.5},
                    {12, 12, 0.75},
                    {14, 12, 0.75},
                    {15, 12, 0.75},
                    {16, 12, 0.75},
                    {18, 12, 0.75},
                    {20, 12, 0.75},
                    {24, 12, 0.75},
                    {30, 12, 0.75},
                    {32, 12, 0.75},
                    {36, 12, 0.75},
                    {40, 12, 0.75},
                    {42, 12, 0.75},
                    {48, 12, 0.75},
                    {60, 12, 0.75},
                    {12, 10, 1.0},
                    {14, 10, 1.0},
                    {15, 10, 1.0},
                    {16, 10, 1.0},
                    {18, 10, 1.0},
                    {20, 10, 1.0},
                    {24, 10, 1.0},
                    {25, 10, 1.0},
                    {28, 10, 1.0},
                    {30, 10, 1.0},
                    {35, 10, 1.0},
                    {40, 10, 1.0},
                    {48, 10, 1.0},
                    {60, 10, 1.0},
                    {12, 8, 1.25},
                    {14, 8, 1.25},
                    {16, 8, 1.25},
                    {17, 8, 1.25},
                    {18, 8, 1.25},
                    {20, 8, 1.25},
                    {22, 8, 1.25},
                    {24, 8, 1.25},
                    {28, 8, 1.25},
                    {32, 8, 1.25},
                    {36, 8, 1.25},
                    {40, 8, 1.25},
                    {12, 6, 1.5},
                    {14, 6, 1.5},
                    {15, 6, 1.5},
                    {16, 6, 1.5},
                    {18, 6, 1.5},
                    {20, 6, 1.5},
                    {22, 6, 1.5},
                    {24, 6, 1.5},
                    {30, 6, 1.5},
                    {36, 6, 1.5},
                    {48, 6, 1.5}}
            };
            defaults.Add(steel145);
            #endregion
            #region stainless steel20
            GearFamily ssteel20 = new GearFamily()
            {
                
                label = "stainless steel20",
                details = "a generic class of stainless steel gears with pa 20",
                Sfb = 50000,
                Sfc = 100000,
                density = 0.283,//ib/cu in
                gears = new double[,]          
                {   {36, 64, 0.1875},
                    {48, 64, 0.1875},
                    {30, 48, 0.1875},
                    {36, 48, 0.1875},
                    {48, 48, 0.1875},
                    {20, 32, 0.1875},
                    {24, 32, 0.1875},
                    {32, 32, 0.1875},
                    {36, 32, 0.1875},
                    {48, 32, 0.1875},
                    {64, 32, 0.1875},
                    {72, 32, 0.1875},
                    {15, 24, 0.1875},
                    {18, 24, 0.1875},
                    {24, 24, 0.1875},
                    {36, 24, 0.1875},
                    {48, 24, 0.1875},
                    {60, 24, 0.3125}}
            };
            defaults.Add(ssteel20);
            #endregion
            #region steel20
            GearFamily steel20 = new GearFamily()
            {
                label = "typeB",
                //label = "steel20",
                details = "a generic class of steel gears with pa 20",
                Sfb = 40000,
                Sfc = 155000,
                density = 0.283,//ib/cu in
                gears = new double[,]          
                {   {15, 20, 0.5},
                    {20, 20, 0.5},
                    {25, 20, 0.5},
                    {30, 20, 0.5},
                    {35, 20, 0.5},
                    {40, 20, 0.5},
                    {16, 16, 0.75},
                    {24, 16, 0.75},
                    {32, 16, 0.75},
                    {48, 16, 0.75},
                    {80, 16, 0.75},
                    {12, 12, 1.0},
                    {15, 12, 1.0},
                    {21, 12, 1.0},
                    {24, 12, 1.0},
                    {36, 12, 1.0},
                    {48, 12, 1.0},
                    {12, 8, 1.5},
                    {16, 8, 1.5},
                    {20, 8, 1.5},
                    {24, 8, 1.5},
                    {28, 8, 1.5}}
            };
            defaults.Add(steel20);
            #endregion
            #region brass20
            GearFamily brass20 = new GearFamily()
            {
                //label = "typeA",
                label = "brass20",
                details = "a generic class of brass gears with pa 20",
                Sfb = 20000,
                Sfc = 65000,
                density = 0.283, //lb/in^3
                gears = new double[,]     
                //teeth, pitch, facewidth
                {   {16, 64, 0.125},
                    {24, 64, 0.125},
                    {28, 64, 0.125},
                    {12, 48, 0.125},
                    {15, 48, 0.125},
                    {18, 48, 0.125},
                    {24, 48, 0.125},
                    {36, 48, 0.125},
                    {48, 48, 0.125},
                    {12, 32, 0.1875},
                    {14, 32, 0.1875},
                    {16, 32, 0.1875},
                    {20, 32, 0.1875},
                    {24, 32, 0.1875},
                    {28, 32, 0.1875},
                    {36, 32, 0.1875},
                    {40, 32, 0.1875},
                    {48, 32, 0.1875},
                    {12, 24, 0.3125},
                    {15, 24, 0.3125},
                    {18, 24, 0.3125},
                    {21, 24, 0.3125},
                    {36, 24, 0.3125},
                    {48, 24, 0.3125}}
            };
            defaults.Add(brass20);
            #endregion
            #region worm gear family 14.5
            GearFamily wormgear145 = new GearFamily()
            {
                label = "wormgear",
                details = "cast iron plain bore",
                Sfb = 15000,
                Sfc = 30000,

                density = 0.283,//ib/cu in
                gears = new double[,]       
                //teeth, pitch, facewidth
                {   {18, 12, 1.125},
                    {20, 12, 1.125},
                    {30, 12, 1.125},
                    {40, 12, 1.25},
                    {60, 12, 1.25},
                    {20, 10, 1.375},
                    {30, 10, 1.375},
                    {40, 10, 1.375},
                    {50, 10, 1.375},
                    {100, 10, 1.375},
                    {20, 8, 1.5},
                    {30, 8, 1.5},
                    {40, 8, 1.625},
                    {20, 6, 1.875},
                    {24, 6, 1.875},
                    {30, 6, 1.875},
                    {40, 6, 1.875},
                    {72, 6, 2.25}}
            };
            defaults.Add(wormgear145);
            #endregion
            #region worm family 14.5
            GearFamily worm145 = new GearFamily()
            {
                label = "worm",
                details = "steel worm with 14.5 PA and right hand threads",
                Sfb = 50000,
                Sfc = 100000,

                density = 0.283,//ib/cu in
                gears = new double[,]       
                //Outerdiameter!!, pitch, facewidth
                {   {1.17, 12, 1.125},
                    {1.45, 10, 1.375},
                    {1.75, 8, 1.75},
                    {2.33, 6, 2.5}}
            };
            defaults.Add(worm145);
            #endregion
            #region nylon bevel gear family20
            GearFamily nylonbevel = new GearFamily()
            {
                label = "bevelA",
                details = "bevel gears with a PA 20",
                Sfb = 5500,
                Sfc = 11000,

                density = 0.04,//ib/cu in
                gears = new double[,]       
                //teeth, pitch, facewidth
                {   {18, 48, 0.28125},
                    {24, 48, 0.375},
                    {16, 32, 0.34375},
                    {24, 32, 0.40625},
                    {24, 24, 0.5625},
                    {30, 24, 0.578125},
                    {36, 24, 0.609375},
                    {16, 16, 0.75}}
            };
            defaults.Add(nylonbevel);
            #endregion
            #region steel bevel gear family20
            GearFamily steelbevel = new GearFamily()
            {
                label = "bevelB",
                details = "steel bevel gears with a PA 20",
                Sfb = 30000,
                Sfc = 60000,

                density = 0.283,//ib/cu in
                gears = new double[,]       
                //teeth, pitch, facewidth
                {   {16, 16, 0.75},
                    {20, 16, 0.84375},
                    {24, 16, 0.875},
                    {15, 12, 0.859375},
                    {18, 12, 1.015625},
                    {21, 12, 1.1875},
                    {24, 12, 1.21875},
                    {30, 12, 1.484375},
                    {20, 10, 1.359375},
                    {25, 10, 1.625}}
            };
            defaults.Add(steelbevel);
            #endregion
            //props for worms, bevels!!!!! sfb, sfc, density
            return defaults;
        }
    }
}
