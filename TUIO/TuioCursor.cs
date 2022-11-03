/*
 TUIO C# Library - part of the reacTIVision project
 Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>

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
     * <summary>
     * The TuioCursor class encapsulates /tuio/2Dcur TUIO cursors.</summary>
     *
     * @author Martin Kaltenbrunner
     * @version 1.1.6
     */
    public class TuioCursor : TuioContainer
    {

        /**
         * <summary>
         * The individual cursor ID number that is assigned to each TuioCursor.</summary>
         */
        protected int cursor_id;

        #region Constructors

        /**
         * <summary>
         * This constructor takes a TuioTime argument and assigns it along with the provided
         * Session ID, Cursor ID, X and Y coordinate to the newly created TuioCursor.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="si">the Session ID to assign</param>
         * <param name="ci">the Cursor ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         */
        public TuioCursor(TuioTime ttime, long si, int ci, float xp, float yp)
            : base(ttime, si, xp, yp,0)
        {
            cursor_id = ci;
        }

        /**
         * <summary>
         * This constructor takes the provided Session ID, Cursor ID, X and Y coordinate
         * and assigs these values to the newly created TuioCursor.</summary>
         *
         * <param name="si">the Session ID to assign</param>
         * <param name="ci">the Cursor ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         */
        public TuioCursor(long si, int ci, float xp, float yp)
            : base(si, xp, yp,0)
        {
            cursor_id = ci;
        }

        /**
         * <summary>
         * This constructor takes the atttibutes of the provided TuioCursor
         * and assigs these values to the newly created TuioCursor.</summary>
         *
         * <param name="tcur">the TuioCursor to assign</param>
         */
        public TuioCursor(TuioCursor tcur)
            : base(tcur)
        {
            cursor_id = tcur.CursorID;
        }
        #endregion



        #region Update Methods

        /**
         * <summary>
         * Takes a TuioTime argument and assigns it along with the provided
         * X and Y coordinate, angle, X and Y velocity, motion acceleration,
         * rotation speed and rotation acceleration to the private TuioObject attributes.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="xs">the X velocity to assign</param>
         * <param name="ys">the Y velocity to assign</param>
         * <param name="ma">the motion acceleration to assign</param>
         */
        public void update(TuioTime ttime, float xp, float yp, float xs, float ys, float ma)
        {
            base.update(ttime, xp, yp, 0, xs, ys, 0, ma);
        }

        /**
         * <summary>
         * Assigns the provided X and Y coordinate, angle, X and Y velocity, motion acceleration
         * rotation velocity and rotation acceleration to the private TuioContainer attributes.
         * The TuioTime time stamp remains unchanged.</summary>
         *
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="xs">the X velocity to assign</param>
         * <param name="ys">the Y velocity to assign</param>
         * <param name="ma">the motion acceleration to assign</param>
         */
        public void update(float xp, float yp, float xs, float ys, float ma)
        {
            base.update(xp, yp, 0, xs, ys, 0, ma);
        }

        /**
         * <summary>
         * Takes a TuioTime argument and assigns it along with the provided
         * X and Y coordinate and angle to the private TuioObject attributes.
         * The speed and accleration values are calculated accordingly.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         */
        public void update(TuioTime ttime, float xp, float yp)
        {
            TuioPoint lastPoint = path.Last.Value;
            base.update(ttime, xp, yp, 0);
        }

        /**
         * <summary>
         * Takes the atttibutes of the provided TuioCursor
         * and assigs these values to this TuioCursor.
         * The TuioTime time stamp of this TuioContainer remains unchanged.</summary>
         *
         * <param name="tcur">the TuioContainer to assign</param>
         */
        public void update(TuioCursor tcur)
        {
            base.update(tcur);
            state = tcur.state;
        }

        /**
         * <summary>
         * This method is used to calculate the speed and acceleration values of a
         * TuioCursor.</summary>
         */
        public new void stop(TuioTime ttime)
        {
            update(ttime, this.xpos, this.ypos);
        }
        #endregion



        #region Properties & Getter/Setter Methods

        /**
         * <summary>
         * Returns the Cursor ID of this TuioCursor.</summary>
         * <returns>the Cursor ID of this TuioCursor</returns>
         */
        public int CursorID
        {
            get { return cursor_id; }
        }

        [Obsolete("This method has been depracated and is provided only for compatability with legacy code. The CursorID property should be used instead.")]
        public int getCursorID()
        {            
            return cursor_id;
        }
        #endregion

    }
}
