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
using System.Collections.Generic;

namespace TUIO
{

    /**
     * <remarks>The abstract TuioContainer class defines common attributes that apply 
     * to both subclasses (TuioObject and TuioCursor).</remarks>
     * <seealso cref="TuioObject"/>
     * <seealso cref="TuioCursor"/>
     *
     * @author Martin Kaltenbrunner
     * @version 1.1.7
     */
    public abstract class TuioContainer : TuioPoint
    {

        /**
         * <summary>
         * The unique session ID number that is assigned to each TUIO object or cursor.</summary>
         */
        protected long session_id;
        /**
         * <summary>
         * The X-axis velocity value.</summary>
         */
        protected float x_speed;
        /**
         * <summary>
         * The Y-axis velocity value.</summary>
         */
        protected float y_speed;
        /**
         * <summary>
         * The Z-axis velocity value.</summary>
         */
        protected float z_speed;
        /**
         * <summary>
         * The motion speed value.</summary>
         */
        protected float motion_speed;
        /**
         * <summary>
         * The motion acceleration value.</summary>
         */
        protected float motion_accel;
        /**
         * <summary>
         * A LinkedList of TuioPoints containing all the previous positions of the TUIO component.</summary>
         */
        protected LinkedList<TuioPoint> path;

        #region State Enumeration Values
        /**
         * <summary>
         * Defines the REMOVED state.</summary>
         */
        public const int TUIO_IDLE = 0;
        /**
         * <summary>
         * Defines the ADDED state.</summary>
         */
        public const int TUIO_ADDED = 1;
        /**
         * <summary>
         * Defines the ACCELERATING state.</summary>
         */
        public const int TUIO_ACCELERATING = 2;
        /**
         * <summary>
         * Defines the DECELERATING state.</summary>
         */
        public const int TUIO_DECELERATING = 3;

        /**
         * <summary>
         * Defines the STOPPED state.</summary>
         */
        public const int TUIO_STOPPED = 5;
        /**
         * <summary>
         * Defines the REMOVED state.</summary>
         */
        public const int TUIO_REMOVED = 6;

        #endregion
        /**
         * <summary>
         * Reflects the current state of the TuioComponent</summary>
         */
        protected int state;

        #region Constructors

        /**
         * <summary>
         * This constructor takes a TuioTime argument and assigns it along with the provided
         * Session ID, X and Y coordinate to the newly created TuioContainer.</summary>
         * 
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="si">the Session ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public TuioContainer(TuioTime ttime, long si, float xp, float yp, float zp)
            : base(ttime, xp, yp,zp)
        {
            session_id = si;
            x_speed = 0.0f;
            y_speed = 0.0f;
            z_speed = 0.0f;
            motion_speed = 0.0f;
            motion_accel = 0.0f;

            path = new LinkedList<TuioPoint>();
            path.AddLast(new TuioPoint(currentTime, xpos, ypos,zpos));
            state = TUIO_ADDED;
        }

        /**
         * <summary>
         * This constructor takes the provided Session ID, X and Y coordinate
         * and assigs these values to the newly created TuioContainer.</summary>
         * 
         * <param name="si">the Session ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         */
        public TuioContainer(long si, float xp, float yp, float zp)
            : base(xp, yp, zp)
        {
            session_id = si;
            x_speed = 0.0f;
            y_speed = 0.0f;
            z_speed = 0.0f;
            motion_speed = 0.0f;
            motion_accel = 0.0f;
            path = new LinkedList<TuioPoint>();
            path.AddLast(new TuioPoint(currentTime, xpos, ypos,zpos));
            state = TUIO_ADDED;
        }

        /**
         * <summary>
         * This constructor takes the atttibutes of the provided TuioContainer
         * and assigs these values to the newly created TuioContainer.</summary>
         * 
         * <param name="tcon">the TuioContainer to assign</param>
         */
        public TuioContainer(TuioContainer tcon)
            : base(tcon)
        {
            session_id = tcon.SessionID;
            x_speed = 0.0f;
            y_speed = 0.0f;
            motion_speed = 0.0f;
            motion_accel = 0.0f;
            path = new LinkedList<TuioPoint>();
            path.AddLast(new TuioPoint(currentTime, xpos, ypos,zpos));
            state = TUIO_ADDED;
        }
        #endregion

        #region Update Methods

        /**
         * <summary>
	     * Takes a TuioTime argument and assigns it along with the provided
	     * X and Y coordinate to the private TuioContainer attributes.
	     * The speed and accleration values are calculated accordingly.</summary>
	     * <param name="ttime">the TuioTime to assign</param>
	     * <param name="xp">the X coordinate to assign</param>
	     * <param name="yp">the Y coordinate to assign</param>
	     * <param name="zp">the Z coordinate to assign</param>
	     */
        public new void update(TuioTime ttime, float xp, float yp, float zp)
        {
            TuioPoint lastPoint = path.Last.Value;
            base.update(ttime, xp, yp, zp);

            TuioTime diffTime = currentTime - lastPoint.TuioTime;
            float dt = diffTime.TotalMilliseconds / 1000.0f;
            float dx = this.xpos - lastPoint.X;
            float dy = this.ypos - lastPoint.Y;
            float dz = this.zpos - lastPoint.Z;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            float last_motion_speed = this.motion_speed;

            this.x_speed = dx / dt;
            this.y_speed = dy / dt;
            this.z_speed = dz / dt;
            this.motion_speed = dist / dt;
            this.motion_accel = (motion_speed - last_motion_speed) / dt;

            if (motion_accel > 0) state = TUIO_ACCELERATING;
            else if (motion_accel < 0) state = TUIO_DECELERATING;
            else state = TUIO_STOPPED;

			lock (path) {
				path.AddLast (new TuioPoint (currentTime, xpos, ypos,zpos));
				if (path.Count > 128) path.RemoveFirst ();
			}
        }

        /**
         * <summary>
         * This method is used to calculate the speed and acceleration values of
         * TuioContainers with unchanged positions.</summary>
         */
        public void stop(TuioTime ttime)
        {
            update(ttime, this.xpos, this.ypos, this.zpos);
        }

        /**
         * <summary>
         * Takes a TuioTime argument and assigns it along with the provided
         * X and Y coordinate, X and Y velocity and acceleration
         * to the private TuioContainer attributes.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Y coordinate to assign</param>
         * <param name="xs">the X velocity to assign</param>
         * <param name="ys">the Y velocity to assign</param>
         * <param name="zs">the Y velocity to assign</param>
         * <param name="ma">the acceleration to assign</param>
         */
        public void update(TuioTime ttime, float xp, float yp, float zp, float xs, float ys, float zs, float ma)
        {
            base.update(ttime, xp, yp,zp);
            x_speed = xs;
            y_speed = ys;
            z_speed = zs;
            motion_speed = (float)Math.Sqrt(x_speed * x_speed + y_speed * y_speed + z_speed * z_speed);
            motion_accel = ma;
           
            if (motion_accel > 0) state = TUIO_ACCELERATING;
            else if (motion_accel < 0) state = TUIO_DECELERATING;
            else state = TUIO_STOPPED;

			lock (path) {
				path.AddLast (new TuioPoint (currentTime, xpos, ypos,zpos));
				if (path.Count > 128) path.RemoveFirst ();
			}
        }

        /**
         * <summary>
         * Assigns the provided X and Y coordinate, X and Y velocity and acceleration
         * to the private TuioContainer attributes. The TuioTime time stamp remains unchanged.</summary>
         *
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         * <param name="xs">the X velocity to assign</param>
         * <param name="ys">the Y velocity to assign</param>
         * <param name="zs">the Z velocity to assign</param>
         * <param name="ma">the acceleration to assign</param>
         */
        public void update(float xp, float yp, float zp, float xs, float ys, float zs, float ma)
        {
            base.update(xp, yp,zp);

            x_speed = xs;
            y_speed = ys;
            z_speed = zs;
            motion_speed = (float)Math.Sqrt(x_speed * x_speed + y_speed * y_speed + z_speed * z_speed);
            motion_accel = ma;
            
            if (motion_accel > 0) state = TUIO_ACCELERATING;
            else if (motion_accel < 0) state = TUIO_DECELERATING;
            else state = TUIO_STOPPED;

			lock (path) {
				path.AddLast (new TuioPoint (currentTime, xpos, ypos,zpos));
				if (path.Count > 128) path.RemoveFirst ();
			}
		}

        /**
         * <summary>
         * Takes the atttibutes of the provided TuioContainer
         * and assigs these values to this TuioContainer.
         * The TuioTime time stamp of this TuioContainer remains unchanged.</summary>
         *
         * <param name="tcon">the TuioContainer to assign</param>
         */
        public void update(TuioContainer tcon)
        {
            base.update(tcon.X, tcon.Y, tcon.Z);

            x_speed = tcon.XSpeed;
            y_speed = tcon.YSpeed;
            z_speed = tcon.ZSpeed;
            motion_speed = tcon.MotionSpeed;
            motion_accel = tcon.MotionAccel;
            
            state = tcon.state;

			lock (path) {
				path.AddLast (new TuioPoint (currentTime, xpos, ypos,zpos));
				if (path.Count > 128) path.RemoveFirst ();
			}
        }
        #endregion

        /**
        * <summary>
        * Assigns the REMOVE state to this TuioContainer and sets
        * its TuioTime time stamp to the provided TuioTime argument.</summary>
        *
        * <param name="ttime">the TuioTime to assign</param>
        */
        public void remove(TuioTime ttime)
        {
            currentTime = ttime;
            state = TUIO_REMOVED;
        }

        #region Properties & Getter/Setter Methods

        /**
         * <summary>
         * Returns the Session ID of this TuioContainer.</summary>
         * <returns>the Session ID of this TuioContainer</returns>
        */
        public long SessionID
        {
            get { return session_id; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property SessionID instead is recommended.")]
        public long getSessionID()
        {
            return SessionID;
        }

        /**
         * <summary>
         * Returns the X velocity of this TuioContainer.</summary>
         * <returns>the X velocity of this TuioContainer</returns>
         */
        public float XSpeed
        {
            get { return x_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property XSpeed instead is recommended.")]
        public float getXSpeed()
        {
            return XSpeed;
        }

        /**
         * <summary>
         * Returns the Y velocity of this TuioContainer.</summary>
         * <returns>the Y velocity of this TuioContainer</returns>
         */
        public float YSpeed
        {
            get { return y_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property YSpeed instead is recommended.")]
        public float getYSpeed()
        {
            return YSpeed;
        }

        /**
         * <summary>
         * Returns the Z velocity of this TuioContainer.</summary>
         * <returns>the Z velocity of this TuioContainer</returns>
         */
        public float ZSpeed
        {
            get { return y_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property ZSpeed instead is recommended.")]
        public float getZSpeed()
        {
            return ZSpeed;
        }

        /**
         * <summary>
         * Returns the position of this TuioContainer.</summary>
         * <returns>the position of this TuioContainer</returns>
         */
        public TuioPoint Position
        {
            get { return new TuioPoint(xpos, ypos,zpos); }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property Position instead is recommended.")]
        public TuioPoint getPosition()
        {
            return Position;
        }

        /**
         * <summary>
         * Returns the path of this TuioContainer.</summary>
         * <returns>the path of this TuioContainer</returns>
         */
        public List<TuioPoint> Path
        {
			get { 
				List<TuioPoint> p;
				lock (path) {
					p = new List<TuioPoint> (path); 
				}
				return p;
			}
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property Path instead is recommended.")]
        public List<TuioPoint> getPath()
        {
            return Path;
        }

        /**
         * <summary>
         * Returns the motion speed of this TuioContainer.</summary>
         * <returns>the motion speed of this TuioContainer</returns>
         */
        public float MotionSpeed
        {
            get { return motion_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property MotionSpeed instead is recommended.")]
        public float getMotionSpeed()
        {
            return MotionSpeed;
        }

        /**
         * <summary>
         * Returns the motion acceleration of this TuioContainer.</summary>
         * <returns>the motion acceleration of this TuioContainer</returns>
         */
        public float MotionAccel
        {
            get { return motion_accel; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property MotionAccel instead is recommended.")]
        public float getMotionAccel()
        {
            return MotionAccel;
        }

        /**
         * <summary>
         * Returns the TUIO state of this TuioContainer.</summary>
         * <returns>the TUIO state of this TuioContainer</returns>
         */
        public int TuioState
        {
            get { return state; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property TuioState instead is recommended.")]
        public int getTuioState()
        {
            return TuioState;
        }

        /**
         * <summary>
         * Returns true of this TuioContainer is moving.</summary>
         * <returns>true of this TuioContainer is moving</returns>
         */
        public virtual bool isMoving
        {
            get
            {
                if ((state == TUIO_ACCELERATING) || (state == TUIO_DECELERATING)) return true;
                else return false;
            }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property isMoving instead is recommended.")]
        public virtual bool getIsMoving()
        {
            return isMoving;
        }

        #endregion

    }
}
