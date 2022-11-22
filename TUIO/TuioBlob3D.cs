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
using System.Collections.Generic;

namespace TUIO
{

    /**
     * <remarks>
     * The TuioBlob3D class encapsulates /tuio/3Dblb TUIO objects.
     * </remarks>
     *
     * @author  Nicolas Bremard
     * @version 1.1.7
     */
    public class TuioBlob3D : TuioContainer
    {
        /**
         * <summary>
         * The individual symbol ID number that is assigned to each TuioBlob3D.</summary>
         */
        protected int blob_id;

        /**
		 * The roll angle value.
		 */
        float roll;
        /**
		 * The roll speed value.
		 */
        float roll_speed;
        /**
		 * The pitch angle value.
		 */
        float pitch;
        /**
		 * The pitch speed value.
		 */
        float pitch_speed;

        /**
		 * The yaw angle value.
		 */
        float yaw;
        /**
		 * The yaw speed value.
		 */
        float yaw_speed;
        /**
         * <summary>
         * The blob width value.</summary>
         */
        protected float width;

		/**
         * <summary>
         * The blob height value.</summary>
         */
		protected float height;

        /**
         * <summary>
         * The blob height value.</summary>
         */
        protected float depth;


        /**
         * <summary>
         * The blob volume value.</summary>
         */
        protected float volume;



        /**
         * <summary>
         * The rotation acceleration value.</summary>
         */
        protected float rotation_accel;

        #region State Enumeration Values

        /**
         * <summary>
         * Defines the ROTATING state.</summary>
         */
        public static readonly int TUIO_ROTATING = 4;
        /**
         * <summary>
         * Defines the RESIZED state.</summary>
         */
        public static readonly int TUIO_RESIZED = 7;
        #endregion

        #region Constructors

        /**
         * <summary>
         * This constructor takes a TuioTime argument and assigns it along with the provided
         * Session ID, Symbol ID, X and Y coordinate and angle to the newly created TuioBlob3D.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="si">the Session ID to assign</param>
         * <param name="bi">the Blob ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         * <param name="ro">the roll to assign</param>
         * <param name="po">the pitch to assign</param>
         * <param name="yo">the yaw to assign</param>
         * <param name="w">the width to assign</param>
         * <param name="h">the height to assign</param>
         * <param name="d">the depth to assign</param>
         * <param name="v">the volume to assign</param>
         */
        public TuioBlob3D(TuioTime ttime, long si, int bi, float xp, float yp, float zp, float ro, float po, float yo, float w, float h, float d, float v)
            : base(ttime, si, xp, yp, zp)
        {
            blob_id = bi;

            roll = ro;
            pitch = po;
            yaw = yo;
            roll_speed = 0.0f;
            pitch_speed = 0.0f;
            yaw_speed = 0.0f;

            rotation_accel = 0.0f;
            
            width = w;
            height = h;
            depth = d;
            volume = v;
        }

        /**
         * <summary>
         * This constructor takes the provided Session ID, Symbol ID, X and Y coordinate
         * and angle, and assigs these values to the newly created TuioBlob3D.</summary>
         *
         * <param name="si">the Session ID to assign</param>
         * <param name="bi">the Blob ID to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         * <param name="ro">the roll to assign</param>
         * <param name="po">the pitch to assign</param>
         * <param name="yo">the yaw to assign</param>
         * <param name="w">the width to assign</param>
         * <param name="h">the height to assign</param>
         * <param name="d">the depth to assign</param>
         * <param name="v">the volume to assign</param>
         */
        public TuioBlob3D(long si, int bi, float xp, float yp, float zp, float ro, float po, float yo, float w, float h, float d, float v)
            : base(si, xp, yp, zp)
        {
            blob_id = bi;
            roll = ro;
            pitch = po;
            yaw = yo;
            roll_speed = 0.0f;
            pitch_speed = 0.0f;
            yaw_speed = 0.0f;

            rotation_accel = 0.0f;

            width = w;
            height = h;
            depth = d;
            volume = v;
        }

        /**
         * <summary>
         * This constructor takes the atttibutes of the provided TuioBlob3D
         * and assigs these values to the newly created TuioBlob3D.</summary>
         *
         * <param name="tblb">the TuioBlob3D to assign</param>
         */
        public TuioBlob3D(TuioBlob3D tblb)
            : base(tblb)
        {
            blob_id = tblb.BlobID;
            roll = tblb.Roll;
            pitch = tblb.Pitch;
            yaw = tblb.Yaw;
            width = tblb.Width;
			height = tblb.Height;
            depth = tblb.Depth;
            volume = tblb.Volume;
            roll_speed = tblb.RollSpeed;
            pitch_speed = tblb.PitchSpeed;
            yaw_speed = tblb.YawSpeed;
            rotation_accel = tblb.RotationAccel;
        }
        #endregion

        #region Update Methods

        /**
         * <summary>
         * Takes a TuioTime argument and assigns it along with the provided
         * X and Y coordinate, angle, X and Y velocity, motion acceleration,
         * rotation speed and rotation acceleration to the private TuioBlob3D attributes.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         * <param name="ro">the roll to assign</param>
         * <param name="po">the pitch to assign</param>
         * <param name="yo">the yaw to assign</param>
         * <param name="w">the width to assign</param>
         * <param name="h">the height to assign</param>
         * <param name="d">the depth to assign</param>
         * <param name="v">the volume to assign</param>
         * <param name="xs">the X velocity to assign</param>
         * <param name="ys">the Y velocity to assign</param>
         * <param name="zs">the Y velocity to assign</param>
         * <param name="ros">the roll speed to assign</param>
         * <param name="pos">the pitch speed to assign</param>
         * <param name="yos">the yaw speed to assign</param>
         * <param name="ma">the motion acceleration to assign</param>
         * <param name="ra">the rotation acceleration to assign</param>
         */
        public void update(TuioTime ttime, float xp, float yp, float zp, float ro, float po, float yo, float w, float h, float d, float v, float xs, float ys, float zs, float ros, float pos, float yos, float ma, float ra)
        {
            base.update(ttime, xp, yp, zp, xs, ys, zs, ma);
            roll = ro;
            pitch = po;
            yaw = yo;
            roll_speed = ros;
            yaw_speed = pos;
            pitch_speed = yos;

            float dw = width - w;
            float dh = height - h;
            float dd = depth - d;
            width = w;
			height = h;
            depth = d;
            volume = v;

            rotation_accel = ra;
            if ((rotation_accel != 0) && (state != TUIO_STOPPED)) state = TUIO_ROTATING;
            if (dw != 0 || dh != 0 || dd != 0) state = TUIO_RESIZED;
        }

        /**
         * <summary>
         * Assigns the provided X and Y coordinate, angle, X and Y velocity, motion acceleration
         * rotation velocity and rotation acceleration to the private TuioContainer attributes.
         * The TuioTime time stamp remains unchanged.</summary>
         *
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         * <param name="ro">the roll to assign</param>
         * <param name="po">the pitch to assign</param>
         * <param name="yo">the yaw to assign</param>
         * <param name="w">the width to assign</param>
         * <param name="h">the height to assign</param>
         * <param name="d">the depth to assign</param>
         * <param name="v">the volume to assign</param>
         * <param name="xs">the X velocity to assign</param>
         * <param name="ys">the Y velocity to assign</param>
         * <param name="zs">the Y velocity to assign</param>
         * <param name="ros">the roll speed to assign</param>
         * <param name="pos">the pitch speed to assign</param>
         * <param name="yos">the yaw speed to assign</param>
         * <param name="ma">the motion acceleration to assign</param>
         * <param name="ra">the rotation acceleration to assign</param>
         */
        public void update(float xp, float yp, float zp, float ro, float po, float yo, float w, float h, float d, float v, float xs, float ys, float zs, float ros, float pos, float yos, float ma, float ra)
        {
            base.update(xp, yp, zp, xs, ys, zs, ma);
            roll = ro;
            pitch = po;
            yaw = yo;
            roll_speed = ros;
            yaw_speed = pos;
            pitch_speed = yos;

            float dw = width - w;
            float dh = height - h;
            float dd = depth - d;
            width = w;
            height = h;
            depth = d;
            volume = v;
            
            rotation_accel = ra;
            if ((rotation_accel != 0) && (state != TUIO_STOPPED)) state = TUIO_ROTATING;
            if (dw != 0 || dh != 0 || dd != 0) state = TUIO_RESIZED;
        }

        /**
         * <summary>
         * Takes a TuioTime argument and assigns it along with the provided
         * X and Y coordinate and angle to the private TuioBlob3D attributes.
         * The speed and accleration values are calculated accordingly.</summary>
         *
         * <param name="ttime">the TuioTime to assign</param>
         * <param name="xp">the X coordinate to assign</param>
         * <param name="yp">the Y coordinate to assign</param>
         * <param name="zp">the Z coordinate to assign</param>
         * <param name="ro">the roll to assign</param>
         * <param name="po">the pitch to assign</param>
         * <param name="yo">the yaw to assign</param>
         * <param name="w">the width to assign</param>
         * <param name="h">the height to assign</param>
         * <param name="d">the depth to assign</param>
         * <param name="v">the volume to assign</param>
         */
        public void update(TuioTime ttime, float xp, float yp, float zp, float ro, float po, float yo, float w, float h, float d, float v)
        {
			TuioPoint lastPoint = path.Last.Value;
            base.update(ttime, xp, yp, zp);

            float dw = width - w;
            float dh = height - h;
            float dd = depth - d;
            width = w;
			height = h;
            depth = d;
            volume = v;

            TuioTime diffTime = currentTime - lastPoint.TuioTime;
            float dt = diffTime.TotalMilliseconds / 1000.0f;


            float last_roll = roll;
            roll = ro;

            float dro = (roll - last_roll) / (2.0f * (float)Math.PI);
            if (dro > 0.75f) dro -= 1.0f;
            else if (dro < -0.75f) dro += 1.0f;

            float last_roll_speed = roll_speed;
            roll_speed = dro / dt;
            float roll_accel = (yaw_speed - last_roll_speed) / dt;


            float last_pitch = pitch;
            pitch = po;

            float dpo = (pitch - last_pitch) / (2.0f * (float)Math.PI);
            if (dpo > 0.75f) dpo -= 1.0f;
            else if (dpo < -0.75f) dpo += 1.0f;

            float last_pitch_speed = pitch_speed;
            pitch_speed = dpo / dt;
            float pitch_accel = (pitch_speed - last_pitch_speed) / dt;


            float last_yaw = yaw;
            yaw = yo;

            float dyo = (yaw - last_yaw) / (2.0f * (float)Math.PI);
            if (dyo > 0.75f) dyo -= 1.0f;
            else if (dyo < -0.75f) dyo += 1.0f;

            float last_yaw_speed = yaw_speed;
            yaw_speed = dyo / dt;
            float yaw_accel = (yaw_speed - last_yaw_speed) / dt;


            rotation_accel = (roll_accel + pitch_accel + yaw_accel) / 3;
            

            if ((rotation_accel != 0) && (state != TUIO_STOPPED)) state = TUIO_ROTATING;
            if (dw != 0 || dh != 0 || dd != 0) state = TUIO_RESIZED;
        }

        /**
         * <summary>
         * Takes the atttibutes of the provided TuioBlob3D
         * and assigs these values to this TuioBlob3D.
         * The TuioTime time stamp of this TuioContainer remains unchanged.</summary>
         *
         * <param name="tblb">the TuioContainer to assign</param>
         */
        public void update(TuioBlob3D tblb)
        {
            base.update(tblb);

            state = tblb.state;
            
            roll = tblb.Roll;
            pitch = tblb.Pitch;
            yaw = tblb.Yaw;
            width = tblb.Width;
            height = tblb.Height;
            depth = tblb.Depth;
            volume = tblb.Volume;
            roll_speed = tblb.RollSpeed;
            pitch_speed = tblb.PitchSpeed;
            yaw_speed = tblb.YawSpeed;
            rotation_accel = tblb.RotationAccel;
        }

        /**
         * <summary>
         * This method is used to calculate the speed and acceleration values of a
         * TuioBlob3D with unchanged position and angle.</summary>
         */
        public new void stop(TuioTime ttime)
        {
            update(ttime, this.xpos, this.ypos, this.zpos, this.roll, this.pitch, this.yaw, this.width, this.height, this.depth, this.volume);
        }
        #endregion

        #region Properties & Getter/Setter Methods

        /**
         * <summary>
         * Returns the symbol ID of this TuioBlob3D.</summary>
         * <returns>the symbol ID of this TuioBlob3D</returns>
         */
        public int BlobID
        {
            get { return blob_id; }
        }
		
		[Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public int getBlobID()
        {
            return BlobID;
        }
	
		/**
         * <summary>
         * Returns the width of this TuioBlob3D.</summary>
         * <returns>the width of this TuioBlob3D</returns>
         */
		public float Width
		{
			get { return width; }
		}

		[Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
		public float getWidth()
		{
			return Width;
		}

		/**
         * <summary>
         * Returns the height of this TuioBlob3D.</summary>
         * <returns>the heigth of this TuioBlob3D</returns>
         */
		public float Height
		{
			get { return height; }
		}

		[Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
		public float getHeight()
		{
			return Height;
		}

		/**
         * <summary>
         * Returns the depth of this TuioBlob3D.</summary>
         * <returns>the depth of this TuioBlob3D</returns>
         */
		public float Depth
		{
			get { return depth; }
		}

		[Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
		public float getDepth()
		{
			return Depth;
		}

        /**
         * <summary>
         * Returns the volume of this TuioBlob3D.</summary>
         * <returns>the volume of this TuioBlob3D</returns>
         */
        public float Volume
        {
            get { return volume; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getVolume()
        {
            return Volume;
        }






        /**
         * <summary>
         * Returns the rotation roll of this TuioBlob3D.</summary>
         * <returns>the rotation roll of this TuioBlob3D</returns>
         */
        public float Roll
        {
            get { return roll; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getRoll()
        {
            return Roll;
        }

        /**
         * <summary>
         * Returns the rotation roll in degrees of this TuioBlob3D.</summary>
         * <returns>the rotation roll in degrees of this TuioBlob3D</returns>
         */
        public float RollDegrees
        {
            get { return roll / (float)Math.PI * 180.0f; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getRollDegrees()
        {
            return RollDegrees;
        }





        /**
         * <summary>
         * Returns the rotation pitch of this TuioBlob3D.</summary>
         * <returns>the rotation pitch of this TuioBlob3D</returns>
         */
        public float Pitch
        {
            get { return pitch; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getPitch()
        {
            return Pitch;
        }

        /**
         * <summary>
         * Returns the rotation pitch in degrees of this TuioBlob3D.</summary>
         * <returns>the rotation pitch in degrees of this TuioBlob3D</returns>
         */
        public float PitchDegrees
        {
            get { return pitch / (float)Math.PI * 180.0f; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getPitchDegrees()
        {
            return PitchDegrees;
        }







        /**
         * <summary>
         * Returns the rotation yaw of this TuioBlob3D.</summary>
         * <returns>the rotation yaw of this TuioBlob3D</returns>
         */
        public float Yaw
        {
            get { return yaw; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getYaw()
        {
            return Yaw;
        }

        /**
         * <summary>
         * Returns the rotation yaw in degrees of this TuioBlob3D.</summary>
         * <returns>the rotation yaw in degrees of this TuioBlob3D</returns>
         */
        public float YawDegrees
        {
            get { return yaw / (float)Math.PI * 180.0f; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getYawDegrees()
        {
            return YawDegrees;
        }






        /**
         * <summary>
         * Returns the roll speed of this TuioBlob3D.</summary>
         * <returns>the roll speed of this TuioBlob3D</returns>
         */
        public float RollSpeed
        {
            get { return roll_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getRollSpeed()
        {
            return RollSpeed;
        }



        /**
         * <summary>
         * Returns the pitch speed of this TuioBlob3D.</summary>
         * <returns>the pitch speed of this TuioBlob3D</returns>
         */
        public float PitchSpeed
        {
            get { return pitch_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getPitchSpeed()
        {
            return PitchSpeed;
        }



        /**
         * <summary>
         * Returns the yaw speed of this TuioBlob3D.</summary>
         * <returns>the yaw speed of this TuioBlob3D</returns>
         */
        public float YawSpeed
        {
            get { return yaw_speed; }
        }

        [Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getYawSpeed()
        {
            return YawSpeed;
        }







        /**
         * <summary>
         * Returns the rotation acceleration of this TuioBlob3D.</summary>
         * <returns>the rotation acceleration of this TuioBlob3D</returns>
         */
        public float RotationAccel
        {
            get { return rotation_accel; }
        }

		[Obsolete("This method is provided only for compatability with legacy code. Use of the property instead is recommended.")]
        public float getRotationAccel()
        {
            return RotationAccel;
        }
		
        /**
         * <summary>
         * Returns true of this TuioBlob3D is moving.</summary>
         * <returns>true of this TuioBlob3D is moving</returns>
         */
        public override bool isMoving
        {
            get
            {
                if ((state == TUIO_ACCELERATING) || (state == TUIO_DECELERATING) || (state == TUIO_ROTATING) || (state == TUIO_RESIZED)) return true;
                else return false;
            }
        }
        #endregion
    }

}
