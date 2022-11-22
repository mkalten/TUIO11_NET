/*
 TUIO C# Library - part of the reacTIVision project
 Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>
Modified by Bremard Nicolas <nicolas@bremard.fr> on 11/2022

 This library is free software; you can redistribute it and/or
 modify it under the terms of the GNU Lesser General Public
 License as published by the Free Software Foundation; either
 version 3.0 of the License, or (at your option) any later version.
 
 This library is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 Lesser General Public License for more details.
 
 You should have received a copy of the GNU Lesser General Public
 License along with this library.
*/

using System;

namespace TUIO
{

    /**
     * The TuioPoint class on the one hand is a simple container and utility class to handle TUIO positions in general,
     * on the other hand the TuioPoint is the base class for the TuioCursor and TuioObject classes.
     *
     * @author Martin Kaltenbrunner
     * @version 1.1.7
     */
    public class TuioPoint
    {
        #region Member Variables

        /**
         * <summary>
         * X coordinate, representated as a floating point value in a range of 0..1</summary>
         */
        protected float xpos;

        /**
         * <summary>
         * Y coordinate, representated as a floating point value in a range of 0..1</summary>
         */
        protected float ypos;

        /**
         * <summary>
         * Z coordinate, representated as a floating point value in a range of 0..1</summary>
         */
        protected float zpos;

        /**
         * <summary>
         * The time stamp of the last update represented as TuioTime (time since session start)</summary>
         */
        protected TuioTime currentTime;

        /**
         * <summary>
         * The creation time of this TuioPoint represented as TuioTime (time since session start)</summary>
         */
        protected TuioTime startTime;

        #endregion

        #region Constructors

        /**
         * <summary>
         * The default constructor takes no arguments and sets
         * its coordinate attributes to zero and its time stamp to the current session time.</summary>
         */
        public TuioPoint()
        {
            xpos = 0.0f;
            ypos = 0.0f;
            zpos = 0.0f;
            currentTime = TuioTime.SessionTime;
            startTime = new TuioTime(currentTime);
        }

        /**
         * <summary>
         * This constructor takes two floating point coordinate arguments and sets
         * its coordinate attributes to these values and its time stamp to the current session time.</summary>
         *
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public TuioPoint(float xp, float yp, float zp)
        {
            xpos = xp;
            ypos = yp;
            zpos = zp;
            currentTime = TuioTime.SessionTime;
            startTime = new TuioTime(currentTime);
        }

        /**
         * <summary>
         * This constructor takes a TuioPoint argument and sets its coordinate attributes
         * to the coordinates of the provided TuioPoint and its time stamp to the current session time.</summary>
         *
         * <param name="tpoint">the TuioPoint to assign</param>
         */
        public TuioPoint(TuioPoint tpoint)
        {
            xpos = tpoint.X;
            ypos = tpoint.Y;
            zpos = tpoint.Z;
            currentTime = TuioTime.SessionTime;
            startTime = new TuioTime(currentTime);
        }

        /**
         * <summary>
         * This constructor takes a TuioTime object and two floating point coordinate arguments and sets
         * its coordinate attributes to these values and its time stamp to the provided TUIO time object.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public TuioPoint(TuioTime ttime, float xp, float yp, float zp)
        {
            xpos = xp;
            ypos = yp;
            zpos = zp;
            currentTime = new TuioTime(ttime);
            startTime = new TuioTime(currentTime);
        }

        #endregion

        #region Update Methods

        /**
         * <summary>
         * Takes a TuioPoint argument and updates its coordinate attributes
         * to the coordinates of the provided TuioPoint and leaves its time stamp unchanged.</summary>
         *
         * <param name="tpoint">the TuioPoint to assign</param>
         */
        public void update(TuioPoint tpoint)
        {
            xpos = tpoint.X;
            ypos = tpoint.Y;
            zpos = tpoint.Z;
        }

        /**
         * <summary>
         * Takes two floating point coordinate arguments and updates its coordinate attributes
         * to the coordinates of the provided TuioPoint and leaves its time stamp unchanged.</summary>
         *
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public void update(float xp, float yp, float zp)
        {
            xpos = xp;
            ypos = yp;
            zpos = zp;
        }

        /**
         * <summary>
         * Takes a TuioTime object and two floating point coordinate arguments and updates its coordinate attributes
         * to the coordinates of the provided TuioPoint and its time stamp to the provided TUIO time object.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public void update(TuioTime ttime, float xp, float yp, float zp)
        {
            xpos = xp;
            ypos = yp;
            zpos = zp;
            currentTime = new TuioTime(ttime);
        }

        #endregion

        #region Properties & Getter/Setter Methods

        /**
         * <summary>
         * Returns the X coordinate of this TuioPoint.</summary>
         * <returns>the X coordinate of this TuioPoint</returns>
         */
        public float X
        {
            get { return xpos; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getX()
        {
            return X;
        }

        /**
         * <summary>
         * Returns the Y coordinate of this TuioPoint.</summary>
         * <returns>the Y coordinate of this TuioPoint</returns>
         */
        public float Y
        {
            get { return ypos; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getY()
        {
            return Y;
        }

        /**
         * <summary>
         * Returns the Z coordinate of this TuioPoint.</summary>
         * <returns>the Z coordinate of this TuioPoint</returns>
         */
        public float Z
        {
            get { return zpos; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getZ()
        {
            return Z;
        }

        /**
         * <summary>
         * Returns the distance to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the distance to the provided coordinates</returns>
         */
        public float getDistance(float xp, float yp)
        {
            float dx = xpos - xp;
            float dy = ypos - yp;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }


        /**
         * <summary>
         * Returns the distance to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <param name="zp">the Z coordinate of the distant point</param>
         * <returns>the distance to the provided coordinates</returns>
         */
        public float getDistance(float xp, float yp, float zp)
        {
            float dx = xpos - xp;
            float dy = ypos - yp;
            float dz = zpos - zp;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /**
         * <summary>
         * Returns the distance to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the distance to the provided TuioPoint</returns>
         */
        public float getDistance(TuioPoint tpoint)
        {
            return getDistance(tpoint.X, tpoint.Y, tpoint.Z);
        }


        /**
         * <summary>
         * Returns the distance to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <param name="zp">the Z coordinate of the distant point</param>
         * <returns>the distance to the provided coordinates</returns>
         */
        public float getSpaceDistance(float xp, float yp, float zp, int w, int h, int d)
        {
            float dx = w * xpos - w * xp;
            float dy = h * ypos - h * yp;
            float dz = d * zpos - d * zp;
            return (float) Math.Sqrt(dx* dx + dy* dy + dz* dz);
        }



        /**
         * <summary>
         * Returns the angle to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle to the provided coordinates</returns>
         */
        public float getAngle(float xp, float yp)
        {

            float side = xp - xpos;
            float height = yp - ypos;
            float distance = getDistance(xp, yp);

            float angle = (float)(Math.Asin(side / distance) + Math.PI / 2);
            if (height < 0) angle = 2.0f * (float)Math.PI - angle;

            return angle;
        }

        /**
         * <summary>
         * Returns the angle to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle to the provided TuioPoint</returns>
         */
        public float getAngle(TuioPoint tpoint)
        {
            return getAngle(tpoint.X, tpoint.Y);
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getAngleDegrees(float xp, float yp)
        {
            return (getAngle(xp, yp) / (float)Math.PI) * 180.0f;
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getAngleDegrees(TuioPoint tpoint)
        {
            return (getAngle(tpoint) / (float)Math.PI) * 180.0f;
        }





        /**
         * <summary>
         * Returns the angle to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle to the provided coordinates</returns>
         */
        public float getRollAngle(float xp, float yp, float zp)
        {
            return getAngle(xp,yp);
        }

        /**
         * <summary>
         * Returns the angle to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle to the provided TuioPoint</returns>
         */
        public float getRollAngle(TuioPoint tpoint)
        {
            return getAngle(tpoint.X, tpoint.Y);
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getRollAngleDegrees(float xp, float yp, float zp)
        {
            return (getAngle(xp, yp) / (float)Math.PI) * 180.0f;
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getRollAngleDegrees(TuioPoint tpoint)
        {
            return (getRollAngle(tpoint) / (float)Math.PI) * 180.0f;
        }




        /**
         * <summary>
         * Returns the angle to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle to the provided coordinates</returns>
         */
        public float getPitchAngle(float xp, float yp, float zp)
        {
            return getAngle(zp, yp);
        }

        /**
         * <summary>
         * Returns the angle to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle to the provided TuioPoint</returns>
         */
        public float getPitchAngle(TuioPoint tpoint)
        {
            return getAngle(tpoint.Z, tpoint.Y);
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getPitchAngleDegrees(float xp, float yp, float zp)
        {
            return (getAngle(zp, yp) / (float)Math.PI) * 180.0f;
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getPitchAngleDegrees(TuioPoint tpoint)
        {
            return (getPitchAngle(tpoint) / (float)Math.PI) * 180.0f;
        }




        /**
         * <summary>
         * Returns the angle to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle to the provided coordinates</returns>
         */
        public float getYawAngle(float xp, float yp, float zp)
        {
            return getAngle(xp, zp);
        }

        /**
         * <summary>
         * Returns the angle to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle to the provided TuioPoint</returns>
         */
        public float getYawAngle(TuioPoint tpoint)
        {
            return getAngle(tpoint.X, tpoint.Z);
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided coordinates</summary>
         *
         * <param name="xp">the X coordinate of the distant point</param>
         * <param name="yp">the Y coordinate of the distant point</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getYawAngleDegrees(float xp, float yp, float zp)
        {
            return (getAngle(xp, zp) / (float)Math.PI) * 180.0f;
        }

        /**
         * <summary>
         * Returns the angle in degrees to the provided TuioPoint</summary>
         *
         * <param name="tpoint">the distant TuioPoint</param>
         * <returns>the angle in degrees to the provided TuioPoint</returns>
         */
        public float getYawAngleDegrees(TuioPoint tpoint)
        {
            return (getYawAngle(tpoint) / (float)Math.PI) * 180.0f;
        }




       


        /**
         * <summary>
         * Returns the X coordinate in pixels relative to the provided screen width.</summary>
         *
         * <param name="width">the screen width</param>
         * <returns>the X coordinate of this TuioPoint in pixels relative to the provided screen width</returns>
         */
        public int getScreenX(int width)
        {
            return (int)Math.Round(xpos * width);
        }

        /**
         * <summary>
         * Returns the Y coordinate in pixels relative to the provided screen height.</summary>
         *
         * <param name="height">the screen height</param>
         * <returns>the Y coordinate of this TuioPoint in pixels relative to the provided screen height</returns>
         */
        public int getScreenY(int height)
        {
            return (int)Math.Round(ypos * height);
        }


        /**
         * <summary>
         * Returns the Y coordinate in pixels relative to the provided screen height.</summary>
         *
         * <param name="height">the screen height</param>
         * <returns>the Y coordinate of this TuioPoint in pixels relative to the provided screen height</returns>
         */
        public int getSpaceX(int width)
        {
            return (int)Math.Round(xpos * width);
        }

        /**
         * <summary>
         * Returns the Y coordinate in pixels relative to the provided screen height.</summary>
         *
         * <param name="height">the screen height</param>
         * <returns>the Y coordinate of this TuioPoint in pixels relative to the provided screen height</returns>
         */
        public int getSpaceY(int height)
        {
            return (int)Math.Round(ypos * height);
        }

        /**
         * <summary>
         * Returns the Y coordinate in pixels relative to the provided screen height.</summary>
         *
         * <param name="height">the screen height</param>
         * <returns>the Y coordinate of this TuioPoint in pixels relative to the provided screen height</returns>
         */
        public int getSpaceZ(int depth)
        {
            return (int)Math.Round(zpos * depth);
        }


        /**
         * <summary>
         * Returns the time stamp of this TuioPoint as TuioTime.</summary>
         *
         * <returns>the time stamp of this TuioPoint as TuioTime</returns>
         */
        public TuioTime TuioTime
        {
            get { return new TuioTime(currentTime); }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public TuioTime getTuioTime()
        {
            return TuioTime;
        }

        /**
         * <summary>
         * Returns the start time of this TuioPoint as TuioTime.</summary>
         *
         * <returns>the start time of this TuioPoint as TuioTime</returns>
         */
        public TuioTime StartTime
        {
            get { return new TuioTime(startTime); }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public TuioTime getStartTime()
        {
            return StartTime;
        }

        #endregion

    }
}
