/*
 TUIO C# Library - part of the reacTIVision project
 Copyright (c) 2022 Nicolas Bremard <nicolas@bremard.fr>
 Based on TuioBlob by Martin Kaltenbrunner <martin@tuio.org>

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
     * The TuioCursor3D class encapsulates /tuio/3Dcur TUIO cursors.</summary>
     *
     * @authorNicolas Bremard
     * @version 1.1.7
     */
    public class TuioCursor3D : TuioContainer
    {

        /**
         * <summary>
         * The individual cursor ID number that is assigned to each TuioCursor3D.</summary>
         */
        protected int cursor_id;

        #region Constructors

        /**
         * <summary>
         * This constructor takes a TuioTime argument and assigns it along with the provided
         * Session ID, Cursor ID, X and Y coordinate to the newly created TuioCursor3D.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="si">the Session ID to assign</param>
         * <param name="ci">the Cursor ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public TuioCursor3D(TuioTime ttime, long si, int ci, float xp, float yp, float zp)
            : base(ttime, si, xp, yp,zp)
        {
            cursor_id = ci;
        }

        /**
         * <summary>
         * This constructor takes the provided Session ID, Cursor ID, X and Y coordinate
         * and assigs these values to the newly created TuioCursor3D.</summary>
         *
         * <param name="si">the Session ID to assign</param>
         * <param name="ci">the Cursor ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public TuioCursor3D(long si, int ci, float xp, float yp, float zp)
            : base(si, xp, yp,zp)
        {
            cursor_id = ci;
        }

        /**
         * <summary>
         * This constructor takes the atttibutes of the provided TuioCursor3D
         * and assigs these values to the newly created TuioCursor3D.</summary>
         *
         * <param name="tcur">the TuioCursor3D to assign</param>
         */
        public TuioCursor3D(TuioCursor3D tcur)
            : base(tcur)
        {
            cursor_id = tcur.CursorID;
        }
        #endregion



        #region Properties & Getter/Setter Methods

        /**
         * <summary>
         * Returns the Cursor ID of this TuioCursor3D.</summary>
         * <returns>the Cursor ID of this TuioCursor3D</returns>
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
