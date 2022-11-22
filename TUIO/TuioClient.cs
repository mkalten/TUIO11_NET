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
using System.Threading;
using System.Collections;
using System.Collections.Generic;

using OSC.NET;

namespace TUIO
{
    /**
     * <remarks>
     * The TuioClient class is the central TUIO protocol decoder component. It provides a simple callback infrastructure using the {@link TuioListener} interface.
	 * In order to receive and decode TUIO messages an instance of TuioClient needs to be created. The TuioClient instance then generates TUIO events
	 * which are broadcasted to all registered classes that implement the {@link TuioListener} interface.
     * </remarks>
     * <example>
     * <code>
     * TuioClient client = new TuioClient();
	 * client.addTuioListener(myTuioListener);
	 * client.start();
     * </code>
     * </example>
     * 
     * @author Martin Kaltenbrunner
     * @version 1.1.7
     */
    public class TuioClient
    {
        private bool connected = false;
        private int port = 3333;
        private OSCReceiver receiver;
        private Thread thread;

        private object cursorSync = new object();
        private object objectSync = new object();
		private object blobSync = new object();

        private Dictionary<long, TuioObject> objectList = new Dictionary<long, TuioObject>(32);
        private List<long> aliveObjectList = new List<long>(32);
        private List<long> newObjectList = new List<long>(32);
        private Dictionary<long, TuioCursor> cursorList = new Dictionary<long, TuioCursor>(32);
        private List<long> aliveCursorList = new List<long>(32);
        private List<long> newCursorList = new List<long>(32);
		private Dictionary<long, TuioBlob> blobList = new Dictionary<long, TuioBlob>(32);
		private List<long> aliveBlobList = new List<long>(32);
		private List<long> newBlobList = new List<long>(32);
        private List<TuioObject> frameObjects = new List<TuioObject>(32);
        private List<TuioCursor> frameCursors = new List<TuioCursor>(32);
		private List<TuioBlob> frameBlobs = new List<TuioBlob>(32);

        private List<TuioCursor> freeCursorList = new List<TuioCursor>();
        private int maxCursorID = -1;
		private List<TuioBlob> freeBlobList = new List<TuioBlob>();
		private int maxBlobID = -1;



        //25D

        private object cursor25DSync = new object();
        private object object25DSync = new object();
        private object blob25DSync = new object();

        private Dictionary<long, TuioObject25D> object25DList = new Dictionary<long, TuioObject25D>(32);
        private List<long> aliveObject25DList = new List<long>(32);
        private List<long> newObject25DList = new List<long>(32);
        private Dictionary<long, TuioCursor25D> cursor25DList = new Dictionary<long, TuioCursor25D>(32);
        private List<long> aliveCursor25DList = new List<long>(32);
        private List<long> newCursor25DList = new List<long>(32);
        private Dictionary<long, TuioBlob25D> blob25DList = new Dictionary<long, TuioBlob25D>(32);
        private List<long> aliveBlob25DList = new List<long>(32);
        private List<long> newBlob25DList = new List<long>(32);
        private List<TuioObject25D> frameObjects25D = new List<TuioObject25D>(32);
        private List<TuioCursor25D> frameCursors25D = new List<TuioCursor25D>(32);
        private List<TuioBlob25D> frameBlobs25D = new List<TuioBlob25D>(32);

        private List<TuioCursor25D> freeCursor25DList = new List<TuioCursor25D>();
        private int maxCursor25DID = -1;
        private List<TuioBlob25D> freeBlob25DList = new List<TuioBlob25D>();
        private int maxBlob25DID = -1;





        //3D

        private object cursor3DSync = new object();
        private object object3DSync = new object();
        private object blob3DSync = new object();

        private Dictionary<long, TuioObject3D> object3DList = new Dictionary<long, TuioObject3D>(32);
        private List<long> aliveObject3DList = new List<long>(32);
        private List<long> newObject3DList = new List<long>(32);
        private Dictionary<long, TuioCursor3D> cursor3DList = new Dictionary<long, TuioCursor3D>(32);
        private List<long> aliveCursor3DList = new List<long>(32);
        private List<long> newCursor3DList = new List<long>(32);
        private Dictionary<long, TuioBlob3D> blob3DList = new Dictionary<long, TuioBlob3D>(32);
        private List<long> aliveBlob3DList = new List<long>(32);
        private List<long> newBlob3DList = new List<long>(32);
        private List<TuioObject3D> frameObjects3D = new List<TuioObject3D>(32);
        private List<TuioCursor3D> frameCursors3D = new List<TuioCursor3D>(32);
        private List<TuioBlob3D> frameBlobs3D = new List<TuioBlob3D>(32);

        private List<TuioCursor3D> freeCursor3DList = new List<TuioCursor3D>();
        private int maxCursor3DID = -1;
        private List<TuioBlob3D> freeBlob3DList = new List<TuioBlob3D>();
        private int maxBlob3DID = -1;










        private int currentFrame = 0;
        private TuioTime currentTime;

        private List<TuioListener> listenerList = new List<TuioListener>();

        #region Constructors
        /**
         * <summary>
		 * The default constructor creates a client that listens to the default TUIO port 3333</summary>
		 */
        public TuioClient() { }

        /**
         * <summary>
         * This constructor creates a client that listens to the provided port</summary>
         * <param name="port">the listening port number</param>
         */
        public TuioClient(int port)
        {
            this.port = port;
        }
        #endregion

        #region Connection Methods
        /**
		 * <summary>
         * Returns the port number listening to.</summary>
         * <returns>the listening port number</returns>
		 */
        public int getPort()
        {
            return port;
        }

        /**
         * <summary>
         * The TuioClient starts listening to TUIO messages on the configured UDP port
         * All reveived TUIO messages are decoded and the resulting TUIO events are broadcasted to all registered TuioListeners</summary>
         */
        public void connect()
        {

            TuioTime.initSession();
            currentTime = new TuioTime();
            currentTime.reset();

            try
            {
                receiver = new OSCReceiver(port);
                connected = true;
                thread = new Thread(new ThreadStart(listen));
                thread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to connect to port " + port);
                Console.WriteLine(e.Message);
            }
        }

        /**
         * <summary>
         * The TuioClient stops listening to TUIO messages on the configured UDP port</summary>
         */
        public void disconnect()
        {
			connected = false;
            if (receiver != null) receiver.Close();
            receiver = null;

            aliveObjectList.Clear();
            aliveCursorList.Clear();
			aliveBlobList.Clear();
            objectList.Clear();
            cursorList.Clear();
			blobList.Clear();
            frameObjects.Clear();
            frameCursors.Clear();
			frameBlobs.Clear();
			freeCursorList.Clear();
			freeBlobList.Clear();


            aliveObject25DList.Clear();
            aliveCursor25DList.Clear();
            aliveBlob25DList.Clear();
            object25DList.Clear();
            cursor25DList.Clear();
            blob25DList.Clear();
            frameObjects25D.Clear();
            frameCursors25D.Clear();
            frameBlobs25D.Clear();
            freeCursor25DList.Clear();
            freeBlob25DList.Clear();

            aliveObject3DList.Clear();
            aliveCursor3DList.Clear();
            aliveBlob3DList.Clear();
            object3DList.Clear();
            cursor3DList.Clear();
            blob3DList.Clear();
            frameObjects3D.Clear();
            frameCursors3D.Clear();
            frameBlobs3D.Clear();
            freeCursor3DList.Clear();
            freeBlob3DList.Clear();
        }

        /**
         * <summary>
         * Returns true if this TuioClient is currently connected.</summary>
         * <returns>true if this TuioClient is currently connected</returns>
         */
        public bool isConnected() { return connected; }

        private void listen()
        {
            while (connected)
            {
                try
                {
                    OSCPacket packet = receiver.Receive();
                    if (packet != null)
                    {
                        if (packet.IsBundle())
                        {
                            ArrayList messages = packet.Values;
                            for (int i = 0; i < messages.Count; i++)
                            {
                                processMessage((OSCMessage)messages[i]);
                            }
                        }
                        else processMessage((OSCMessage)packet);
                    }
                    else Console.WriteLine("null packet");
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
        #endregion

        /**
		 * <summary>
         * The OSC callback method where all TUIO messages are received and decoded
		 * and where the TUIO event callbacks are dispatched</summary>
         * <param name="message">the received OSC message</param>
		 */
        private void processMessage(OSCMessage message)
        {
            string address = message.Address;
            ArrayList args = message.Values;
            string command = (string)args[0];

            if (address == "/tuio/2Dobj")
            {
                if (command == "set")
                {

                    long s_id = (int)args[1];
                    int f_id = (int)args[2];
                    float xpos = (float)args[3];
                    float ypos = (float)args[4];
                    float angle = (float)args[5];
                    float xspeed = (float)args[6];
                    float yspeed = (float)args[7];
                    float rspeed = (float)args[8];
                    float maccel = (float)args[9];
                    float raccel = (float)args[10];

                    lock (objectSync)
                    {
                        if (!objectList.ContainsKey(s_id))
                        {
                            TuioObject addObject = new TuioObject(s_id, f_id, xpos, ypos, angle);
                            frameObjects.Add(addObject);
                        }
                        else
                        {
                            TuioObject tobj = objectList[s_id];
                            if (tobj == null) return;
                            if ((tobj.X != xpos) || (tobj.Y != ypos) || (tobj.Angle != angle) || (tobj.XSpeed != xspeed) || (tobj.YSpeed != yspeed) || (tobj.RotationSpeed != rspeed) || (tobj.MotionAccel != maccel) || (tobj.RotationAccel != raccel))
                            {

                                TuioObject updateObject = new TuioObject(s_id, f_id, xpos, ypos, angle);
                                updateObject.update(xpos, ypos, angle, xspeed, yspeed, rspeed, maccel, raccel);
                                frameObjects.Add(updateObject);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newObjectList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newObjectList.Add(s_id);
                        // reduce the object list to the lost objects
                        if (aliveObjectList.Contains(s_id))
                            aliveObjectList.Remove(s_id);
                    }

                    // remove the remaining objects
                    lock (objectSync)
                    {
                        for (int i = 0; i < aliveObjectList.Count; i++)
                        {
                            long s_id = aliveObjectList[i];
                            TuioObject removeObject = objectList[s_id];
                            removeObject.remove(currentTime);
                            frameObjects.Add(removeObject);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioObject> frameEnum = frameObjects.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioObject tobj = frameEnum.Current;

                            switch (tobj.TuioState)
                            {
                                case TuioObject.TUIO_REMOVED:
                                    TuioObject removeObject = tobj;
                                    removeObject.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioObject(removeObject);
                                    }
                                    lock (objectSync)
                                    {
                                        objectList.Remove(removeObject.SessionID);
                                    }
                                    break;
                                case TuioObject.TUIO_ADDED:
                                    TuioObject addObject = new TuioObject(currentTime, tobj.SessionID, tobj.SymbolID, tobj.X, tobj.Y, tobj.Angle);
                                    lock (objectSync)
                                    {
                                        objectList.Add(addObject.SessionID, addObject);
                                    }
                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioObject(addObject);
                                    }
                                    break;
                                default:
                                    TuioObject updateObject = getTuioObject(tobj.SessionID);
                                    if ((tobj.X != updateObject.X && tobj.XSpeed == 0) || (tobj.Y != updateObject.Y && tobj.YSpeed == 0) || (tobj.Angle != updateObject.Angle && tobj.RotationSpeed == 0))
                                        updateObject.update(currentTime, tobj.X, tobj.Y, tobj.Angle);
                                    else
                                        updateObject.update(currentTime, tobj.X, tobj.Y, tobj.Angle, tobj.XSpeed, tobj.YSpeed, tobj.RotationSpeed, tobj.MotionAccel, tobj.RotationAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioObject(updateObject);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveObjectList;
                        aliveObjectList = newObjectList;
                        // recycling the List
                        newObjectList = buffer;
                    }
                    frameObjects.Clear();
                }

            }
            else if (address == "/tuio/2Dcur")
            {

                if (command == "set")
                {

                    long s_id = (int)args[1];
                    float xpos = (float)args[2];
                    float ypos = (float)args[3];
                    float xspeed = (float)args[4];
                    float yspeed = (float)args[5];
                    float maccel = (float)args[6];

                    lock (cursorList)
                    {
                        if (!cursorList.ContainsKey(s_id))
                        {

                            TuioCursor addCursor = new TuioCursor(s_id, -1, xpos, ypos);
                            frameCursors.Add(addCursor);

                        }
                        else
                        {
                            TuioCursor tcur = (TuioCursor)cursorList[s_id];
                            if (tcur == null) return;
                            if ((tcur.X != xpos) || (tcur.Y != ypos) || (tcur.XSpeed != xspeed) || (tcur.YSpeed != yspeed) || (tcur.MotionAccel != maccel))
                            {
                                TuioCursor updateCursor = new TuioCursor(s_id, tcur.CursorID, xpos, ypos);
                                updateCursor.update(xpos, ypos, xspeed, yspeed, maccel);
                                frameCursors.Add(updateCursor);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newCursorList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newCursorList.Add(s_id);
                        // reduce the cursor list to the lost cursors
                        if (aliveCursorList.Contains(s_id))
                            aliveCursorList.Remove(s_id);
                    }

                    // remove the remaining cursors
                    lock (cursorSync)
                    {
                        for (int i = 0; i < aliveCursorList.Count; i++)
                        {
                            long s_id = aliveCursorList[i];
                            if (!cursorList.ContainsKey(s_id)) continue;
                            TuioCursor removeCursor = cursorList[s_id];
                            removeCursor.remove(currentTime);
                            frameCursors.Add(removeCursor);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioCursor> frameEnum = frameCursors.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioCursor tcur = frameEnum.Current;
                            switch (tcur.TuioState)
                            {
                                case TuioCursor.TUIO_REMOVED:
                                    TuioCursor removeCursor = tcur;
                                    removeCursor.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioCursor(removeCursor);
                                    }
                                    lock (cursorSync)
                                    {
                                        cursorList.Remove(removeCursor.SessionID);

                                        if (removeCursor.CursorID == maxCursorID)
                                        {
                                            maxCursorID = -1;

                                            if (cursorList.Count > 0)
                                            {

                                                IEnumerator<KeyValuePair<long, TuioCursor>> clist = cursorList.GetEnumerator();
                                                while (clist.MoveNext())
                                                {
                                                    int f_id = clist.Current.Value.CursorID;
                                                    if (f_id > maxCursorID) maxCursorID = f_id;
                                                }

                                                List<TuioCursor> freeCursorBuffer = new List<TuioCursor>();
                                                IEnumerator<TuioCursor> flist = freeCursorList.GetEnumerator();
                                                while (flist.MoveNext())
                                                {
                                                    TuioCursor testCursor = flist.Current;
                                                    if (testCursor.CursorID < maxCursorID) freeCursorBuffer.Add(testCursor);
                                                }
                                                freeCursorList = freeCursorBuffer;
                                            }
                                            else freeCursorList.Clear();
                                        }
                                        else if (removeCursor.CursorID < maxCursorID) freeCursorList.Add(removeCursor);
                                    }
                                    break;

                                case TuioCursor.TUIO_ADDED:
                                    TuioCursor addCursor;
                                    lock (cursorSync)
                                    {
                                        int c_id = cursorList.Count;
                                        if ((cursorList.Count <= maxCursorID) && (freeCursorList.Count > 0))
                                        {
                                            TuioCursor closestCursor = freeCursorList[0];
                                            IEnumerator<TuioCursor> testList = freeCursorList.GetEnumerator();
                                            while (testList.MoveNext())
                                            {
                                                TuioCursor testCursor = testList.Current;
                                                if (testCursor.getDistance(tcur) < closestCursor.getDistance(tcur)) closestCursor = testCursor;
                                            }
                                            c_id = closestCursor.CursorID;
                                            freeCursorList.Remove(closestCursor);
                                        }
                                        else maxCursorID = c_id;

                                        addCursor = new TuioCursor(currentTime, tcur.SessionID, c_id, tcur.X, tcur.Y);
                                        cursorList.Add(addCursor.SessionID, addCursor);
                                    }

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioCursor(addCursor);
                                    }
                                    break;

                                default:
                                    TuioCursor updateCursor = getTuioCursor(tcur.SessionID);
                                    if ((tcur.X != updateCursor.X && tcur.XSpeed == 0) || (tcur.Y != updateCursor.Y && tcur.YSpeed == 0))
                                        updateCursor.update(currentTime, tcur.X, tcur.Y);
                                    else
                                        updateCursor.update(currentTime, tcur.X, tcur.Y, tcur.XSpeed, tcur.YSpeed, tcur.MotionAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioCursor(updateCursor);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveCursorList;
                        aliveCursorList = newCursorList;
                        // recycling the List
                        newCursorList = buffer;
                    }
                    frameCursors.Clear();
                }

            } 
			else if (address == "/tuio/2Dblb")
			{

				if (command == "set")
				{

					long s_id = (int)args[1];
					float xpos = (float)args[2];
					float ypos = (float)args[3];
					float angle = (float)args[4];
					float width = (float)args[5];
					float height = (float)args[6];
					float area = (float)args[7];
					float xspeed = (float)args[8];
					float yspeed = (float)args[9];
					float rspeed = (float)args[10];
					float maccel = (float)args[11];
					float raccel = (float)args[12];

					lock (blobList)
					{
						if (!blobList.ContainsKey(s_id))
						{
							TuioBlob addBlob = new TuioBlob(s_id, -1, xpos, ypos, angle, width, height, area);
							frameBlobs.Add(addBlob);
						}
						else
						{
							TuioBlob tblb = (TuioBlob)blobList[s_id];
							if (tblb == null) return;
							if ((tblb.X != xpos) || (tblb.Y != ypos) || (tblb.Angle != angle) || (tblb.Width != width) || (tblb.Height != height) || (tblb.Area != area) || (tblb.XSpeed != xspeed) || (tblb.YSpeed != yspeed) || (tblb.RotationSpeed != rspeed) || (tblb.MotionAccel != maccel) || (tblb.RotationAccel != raccel))
							{
								TuioBlob updateBlob = new TuioBlob(s_id, tblb.BlobID, xpos, ypos, angle, width, height, area);
								updateBlob.update(xpos, ypos, angle, width, height, area, xspeed, yspeed, rspeed, maccel, raccel);
								frameBlobs.Add(updateBlob);
							}
						}
					}

				}
				else if (command == "alive")
				{

					newBlobList.Clear();
					for (int i = 1; i < args.Count; i++)
					{
						// get the message content
						long s_id = (int)args[i];
						newBlobList.Add(s_id);
						// reduce the blob list to the lost blobs
						if (aliveBlobList.Contains(s_id))
							aliveBlobList.Remove(s_id);
					}

					// remove the remaining blobs
					lock (blobSync)
					{
						for (int i = 0; i < aliveBlobList.Count; i++)
						{
							long s_id = aliveBlobList[i];
							if (!blobList.ContainsKey(s_id)) continue;
							TuioBlob removeBlob = blobList[s_id];
							removeBlob.remove(currentTime);
							frameBlobs.Add(removeBlob);
						}
					}

				}
				else if (command == "fseq")
				{
					int fseq = (int)args[1];
					bool lateFrame = false;

					if (fseq > 0)
					{
						if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
						if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
						else lateFrame = true;
					}
					else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
					{
						currentTime = TuioTime.SessionTime;
					}

					if (!lateFrame)
					{

						IEnumerator<TuioBlob> frameEnum = frameBlobs.GetEnumerator();
						while (frameEnum.MoveNext())
						{
							TuioBlob tblb = frameEnum.Current;
							switch (tblb.TuioState)
							{
							case TuioBlob.TUIO_REMOVED:
								TuioBlob removeBlob = tblb;
								removeBlob.remove(currentTime);

								for (int i = 0; i < listenerList.Count; i++)
								{
									TuioListener listener = (TuioListener)listenerList[i];
									if (listener != null) listener.removeTuioBlob(removeBlob);
								}
								lock (blobSync)
								{
									blobList.Remove(removeBlob.SessionID);

									if (removeBlob.BlobID == maxBlobID)
									{
										maxBlobID = -1;

										if (blobList.Count > 0)
										{

											IEnumerator<KeyValuePair<long, TuioBlob>> blist = blobList.GetEnumerator();
											while (blist.MoveNext())
											{
												int b_id = blist.Current.Value.BlobID;
												if (b_id > maxBlobID) maxBlobID = b_id;
											}

											List<TuioBlob> freeBlobBuffer = new List<TuioBlob>();
											IEnumerator<TuioBlob> flist = freeBlobList.GetEnumerator();
											while (flist.MoveNext())
											{
												TuioBlob testBlob = flist.Current;
												if (testBlob.BlobID < maxBlobID) freeBlobBuffer.Add(testBlob);
											}
											freeBlobList = freeBlobBuffer;
										}
										else freeBlobList.Clear();
									}
									else if (removeBlob.BlobID < maxBlobID) freeBlobList.Add(removeBlob);
								}
								break;

							case TuioBlob.TUIO_ADDED:
								TuioBlob addBlob;
								lock (blobSync)
								{
									int b_id = blobList.Count;
									if ((blobList.Count <= maxBlobID) && (freeBlobList.Count > 0))
									{
										TuioBlob closestBlob = freeBlobList[0];
										IEnumerator<TuioBlob> testList = freeBlobList.GetEnumerator();
										while (testList.MoveNext())
										{
											TuioBlob testBlob = testList.Current;
											if (testBlob.getDistance(tblb) < closestBlob.getDistance(tblb)) closestBlob = testBlob;
										}
										b_id = closestBlob.BlobID;
										freeBlobList.Remove(closestBlob);
									}
									else maxBlobID = b_id;

									addBlob = new TuioBlob(currentTime, tblb.SessionID, b_id, tblb.X, tblb.Y, tblb.Angle, tblb.Width, tblb.Height, tblb.Area);
									blobList.Add(addBlob.SessionID, addBlob);
								}

								for (int i = 0; i < listenerList.Count; i++)
								{
									TuioListener listener = (TuioListener)listenerList[i];
									if (listener != null) listener.addTuioBlob(addBlob);
								}
								break;

							default:
								TuioBlob updateBlob = getTuioBlob(tblb.SessionID);
								if ((tblb.X != updateBlob.X && tblb.XSpeed == 0) || (tblb.Y != updateBlob.Y && tblb.YSpeed == 0) || (tblb.Angle != updateBlob.Angle && tblb.RotationSpeed == 0))
									updateBlob.update(currentTime, tblb.X, tblb.Y, tblb.Angle, tblb.Width, tblb.Height, tblb.Area);
								else
									updateBlob.update(currentTime, tblb.X, tblb.Y, tblb.Angle, tblb.Width, tblb.Height, tblb.Area, tblb.XSpeed, tblb.YSpeed, tblb.RotationSpeed, tblb.MotionAccel, tblb.RotationAccel);

								for (int i = 0; i < listenerList.Count; i++)
								{
									TuioListener listener = (TuioListener)listenerList[i];
									if (listener != null) listener.updateTuioBlob(updateBlob);
								}
								break;
							}
						}

						for (int i = 0; i < listenerList.Count; i++)
						{
							TuioListener listener = (TuioListener)listenerList[i];
							if (listener != null) listener.refresh(new TuioTime(currentTime));
						}

						List<long> buffer = aliveBlobList;
						aliveBlobList = newBlobList;
						// recycling the List
						newBlobList = buffer;
					}
					frameBlobs.Clear();
				}

			}
            else if (address == "/tuio/25Dobj")
            {
                if (command == "set")
                {

                    long s_id = (int)args[1];
                    int f_id = (int)args[2];
                    float xpos = (float)args[3];
                    float ypos = (float)args[4];
                    float zpos = (float)args[5];
                    float angle = (float)args[6];
                    float xspeed = (float)args[7];
                    float yspeed = (float)args[8];
                    float zspeed = (float)args[9];
                    float rspeed = (float)args[10];
                    float maccel = (float)args[11];
                    float raccel = (float)args[12];

                    lock (object25DSync)
                    {
                        if (!object25DList.ContainsKey(s_id))
                        {
                            TuioObject25D addObject = new TuioObject25D(s_id, f_id, xpos, ypos,zpos, angle);
                            frameObjects25D.Add(addObject);
                        }
                        else
                        {
                            TuioObject25D tobj = object25DList[s_id];
                            if (tobj == null) return;
                            if ((tobj.X != xpos) || (tobj.Y != ypos) || (tobj.Z != zpos) || (tobj.Angle != angle) || (tobj.XSpeed != xspeed) || (tobj.YSpeed != yspeed) || (tobj.ZSpeed != zspeed) || (tobj.RotationSpeed != rspeed) || (tobj.MotionAccel != maccel) || (tobj.RotationAccel != raccel))
                            {

                                TuioObject25D updateObject = new TuioObject25D(s_id, f_id, xpos, ypos,zpos, angle);
                                updateObject.update(xpos, ypos,zpos, angle, xspeed, yspeed,zspeed, rspeed, maccel, raccel);
                                frameObjects25D.Add(updateObject);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newObject25DList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newObject25DList.Add(s_id);
                        // reduce the object list to the lost objects
                        if (aliveObject25DList.Contains(s_id))
                            aliveObject25DList.Remove(s_id);
                    }

                    // remove the remaining objects
                    lock (object25DSync)
                    {
                        for (int i = 0; i < aliveObject25DList.Count; i++)
                        {
                            long s_id = aliveObject25DList[i];
                            TuioObject25D removeObject = object25DList[s_id];
                            removeObject.remove(currentTime);
                            frameObjects25D.Add(removeObject);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioObject25D> frameEnum = frameObjects25D.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioObject25D tobj = frameEnum.Current;

                            switch (tobj.TuioState)
                            {
                                case TuioObject25D.TUIO_REMOVED:
                                    TuioObject25D removeObject = tobj;
                                    removeObject.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioObject25D(removeObject);
                                    }
                                    lock (object25DSync)
                                    {
                                        object25DList.Remove(removeObject.SessionID);
                                    }
                                    break;
                                case TuioObject25D.TUIO_ADDED:
                                    TuioObject25D addObject = new TuioObject25D(currentTime, tobj.SessionID, tobj.SymbolID, tobj.X, tobj.Y, tobj.Z, tobj.Angle);
                                    lock (object25DSync)
                                    {
                                        object25DList.Add(addObject.SessionID, addObject);
                                    }
                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioObject25D(addObject);
                                    }
                                    break;
                                default:
                                    TuioObject25D updateObject = getTuioObject25D(tobj.SessionID);
                                    if ((tobj.X != updateObject.X && tobj.XSpeed == 0) || (tobj.Y != updateObject.Y && tobj.YSpeed == 0) || (tobj.Z != updateObject.Z && tobj.ZSpeed == 0) || (tobj.Angle != updateObject.Angle && tobj.RotationSpeed == 0))
                                        updateObject.update(currentTime, tobj.X, tobj.Y, tobj.Z, tobj.Angle);
                                    else
                                        updateObject.update(currentTime, tobj.X, tobj.Y, tobj.Z, tobj.Angle, tobj.XSpeed, tobj.YSpeed, tobj.ZSpeed, tobj.RotationSpeed, tobj.MotionAccel, tobj.RotationAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioObject25D(updateObject);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveObject25DList;
                        aliveObject25DList = newObject25DList;
                        // recycling the List
                        newObject25DList = buffer;
                    }
                    frameObjects25D.Clear();
                }

            }
            else if (address == "/tuio/25Dcur")
            {

                if (command == "set")
                {

                    long s_id = (int)args[1];
                    float xpos = (float)args[2];
                    float ypos = (float)args[3];
                    float zpos = (float)args[4];
                    float xspeed = (float)args[5];
                    float yspeed = (float)args[6];
                    float zspeed = (float)args[7];
                    float maccel = (float)args[8];

                    lock (cursor25DList)
                    {
                        if (!cursor25DList.ContainsKey(s_id))
                        {

                            TuioCursor25D addCursor = new TuioCursor25D(s_id, -1, xpos, ypos,zpos);
                            frameCursors25D.Add(addCursor);

                        }
                        else
                        {
                            TuioCursor25D tcur = (TuioCursor25D)cursor25DList[s_id];
                            if (tcur == null) return;
                            if ((tcur.X != xpos) || (tcur.Y != ypos) || (tcur.Z != zpos) || (tcur.XSpeed != xspeed) || (tcur.YSpeed != yspeed) || (tcur.ZSpeed != zspeed) || (tcur.MotionAccel != maccel))
                            {
                                TuioCursor25D updateCursor = new TuioCursor25D(s_id, tcur.CursorID, xpos, ypos,zpos);
                                updateCursor.update(xpos, ypos,zpos, xspeed, yspeed,zspeed, maccel);
                                frameCursors25D.Add(updateCursor);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newCursor25DList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newCursor25DList.Add(s_id);
                        // reduce the cursor list to the lost cursors
                        if (aliveCursor25DList.Contains(s_id))
                            aliveCursor25DList.Remove(s_id);
                    }

                    // remove the remaining cursors
                    lock (cursor25DSync)
                    {
                        for (int i = 0; i < aliveCursor25DList.Count; i++)
                        {
                            long s_id = aliveCursor25DList[i];
                            if (!cursor25DList.ContainsKey(s_id)) continue;
                            TuioCursor25D removeCursor = cursor25DList[s_id];
                            removeCursor.remove(currentTime);
                            frameCursors25D.Add(removeCursor);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioCursor25D> frameEnum = frameCursors25D.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioCursor25D tcur = frameEnum.Current;
                            switch (tcur.TuioState)
                            {
                                case TuioCursor.TUIO_REMOVED:
                                    TuioCursor25D removeCursor = tcur;
                                    removeCursor.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioCursor25D(removeCursor);
                                    }
                                    lock (cursor25DSync)
                                    {
                                        cursor25DList.Remove(removeCursor.SessionID);

                                        if (removeCursor.CursorID == maxCursor25DID)
                                        {
                                            maxCursor25DID = -1;

                                            if (cursor25DList.Count > 0)
                                            {

                                                IEnumerator<KeyValuePair<long, TuioCursor25D>> clist = cursor25DList.GetEnumerator();
                                                while (clist.MoveNext())
                                                {
                                                    int f_id = clist.Current.Value.CursorID;
                                                    if (f_id > maxCursor25DID) maxCursor25DID = f_id;
                                                }

                                                List<TuioCursor25D> freeCursorBuffer = new List<TuioCursor25D>();
                                                IEnumerator<TuioCursor25D> flist = freeCursor25DList.GetEnumerator();
                                                while (flist.MoveNext())
                                                {
                                                    TuioCursor25D testCursor = flist.Current;
                                                    if (testCursor.CursorID < maxCursor25DID) freeCursorBuffer.Add(testCursor);
                                                }
                                                freeCursor25DList = freeCursorBuffer;
                                            }
                                            else freeCursor25DList.Clear();
                                        }
                                        else if (removeCursor.CursorID < maxCursor25DID) freeCursor25DList.Add(removeCursor);
                                    }
                                    break;

                                case TuioCursor.TUIO_ADDED:
                                    TuioCursor25D addCursor;
                                    lock (cursor25DSync)
                                    {
                                        int c_id = cursor25DList.Count;
                                        if ((cursor25DList.Count <= maxCursor25DID) && (freeCursor25DList.Count > 0))
                                        {
                                            TuioCursor25D closestCursor = freeCursor25DList[0];
                                            IEnumerator<TuioCursor25D> testList = freeCursor25DList.GetEnumerator();
                                            while (testList.MoveNext())
                                            {
                                                TuioCursor25D testCursor = testList.Current;
                                                if (testCursor.getDistance(tcur) < closestCursor.getDistance(tcur)) closestCursor = testCursor;
                                            }
                                            c_id = closestCursor.CursorID;
                                            freeCursor25DList.Remove(closestCursor);
                                        }
                                        else maxCursor25DID = c_id;

                                        addCursor = new TuioCursor25D(currentTime, tcur.SessionID, c_id, tcur.X, tcur.Y, tcur.Z);
                                        cursor25DList.Add(addCursor.SessionID, addCursor);
                                    }

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioCursor25D(addCursor);
                                    }
                                    break;

                                default:
                                    TuioCursor25D updateCursor = getTuioCursor25D(tcur.SessionID);
                                    if ((tcur.X != updateCursor.X && tcur.XSpeed == 0) || (tcur.Y != updateCursor.Y && tcur.YSpeed == 0) || (tcur.Z != updateCursor.Z && tcur.ZSpeed == 0))
                                        updateCursor.update(currentTime, tcur.X, tcur.Y, tcur.Z);
                                    else
                                        updateCursor.update(currentTime, tcur.X, tcur.Y, tcur.Z, tcur.XSpeed, tcur.YSpeed, tcur.ZSpeed, tcur.MotionAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioCursor25D(updateCursor);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveCursor25DList;
                        aliveCursor25DList = newCursor25DList;
                        // recycling the List
                        newCursor25DList = buffer;
                    }
                    frameCursors25D.Clear();
                }

            }
            else if (address == "/tuio/25Dblb")
            {

                if (command == "set")
                {

                    long s_id = (int)args[1];
                    float xpos = (float)args[2];
                    float ypos = (float)args[3];
                    float zpos = (float)args[4];
                    float angle = (float)args[5];
                    float width = (float)args[6];
                    float height = (float)args[7];
                    float area = (float)args[8];
                    float xspeed = (float)args[9];
                    float yspeed = (float)args[10];
                    float zspeed = (float)args[11];
                    float rspeed = (float)args[12];
                    float maccel = (float)args[13];
                    float raccel = (float)args[14];

                    lock (blob25DList)
                    {
                        if (!blob25DList.ContainsKey(s_id))
                        {
                            TuioBlob25D addBlob = new TuioBlob25D(s_id, -1, xpos, ypos,zpos, angle, width, height, area);
                            frameBlobs25D.Add(addBlob);
                        }
                        else
                        {
                            TuioBlob25D tblb = (TuioBlob25D)blob25DList[s_id];
                            if (tblb == null) return;
                            if ((tblb.X != xpos) || (tblb.Y != ypos) || (tblb.Z != zpos) || (tblb.Angle != angle) || (tblb.Width != width) || (tblb.Height != height) || (tblb.Area != area) || (tblb.XSpeed != xspeed) || (tblb.YSpeed != yspeed) || (tblb.ZSpeed != zspeed) || (tblb.RotationSpeed != rspeed) || (tblb.MotionAccel != maccel) || (tblb.RotationAccel != raccel))
                            {
                                TuioBlob25D updateBlob = new TuioBlob25D(s_id, tblb.BlobID, xpos, ypos,zpos, angle, width, height, area);
                                updateBlob.update(xpos, ypos,zpos, angle, width, height, area, xspeed, yspeed,zspeed, rspeed, maccel, raccel);
                                frameBlobs25D.Add(updateBlob);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newBlob25DList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newBlob25DList.Add(s_id);
                        // reduce the blob list to the lost blobs
                        if (aliveBlob25DList.Contains(s_id))
                            aliveBlob25DList.Remove(s_id);
                    }

                    // remove the remaining blobs
                    lock (blob25DSync)
                    {
                        for (int i = 0; i < aliveBlob25DList.Count; i++)
                        {
                            long s_id = aliveBlob25DList[i];
                            if (!blob25DList.ContainsKey(s_id)) continue;
                            TuioBlob25D removeBlob = blob25DList[s_id];
                            removeBlob.remove(currentTime);
                            frameBlobs25D.Add(removeBlob);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioBlob25D> frameEnum = frameBlobs25D.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioBlob25D tblb = frameEnum.Current;
                            switch (tblb.TuioState)
                            {
                                case TuioBlob25D.TUIO_REMOVED:
                                    TuioBlob25D removeBlob = tblb;
                                    removeBlob.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioBlob25D(removeBlob);
                                    }
                                    lock (blob25DSync)
                                    {
                                        blob25DList.Remove(removeBlob.SessionID);

                                        if (removeBlob.BlobID == maxBlob25DID)
                                        {
                                            maxBlob25DID = -1;

                                            if (blob25DList.Count > 0)
                                            {

                                                IEnumerator<KeyValuePair<long, TuioBlob25D>> blist = blob25DList.GetEnumerator();
                                                while (blist.MoveNext())
                                                {
                                                    int b_id = blist.Current.Value.BlobID;
                                                    if (b_id > maxBlob25DID) maxBlob25DID = b_id;
                                                }

                                                List<TuioBlob25D> freeBlobBuffer = new List<TuioBlob25D>();
                                                IEnumerator<TuioBlob25D> flist = freeBlob25DList.GetEnumerator();
                                                while (flist.MoveNext())
                                                {
                                                    TuioBlob25D testBlob = flist.Current;
                                                    if (testBlob.BlobID < maxBlob25DID) freeBlobBuffer.Add(testBlob);
                                                }
                                                freeBlob25DList = freeBlobBuffer;
                                            }
                                            else freeBlob25DList.Clear();
                                        }
                                        else if (removeBlob.BlobID < maxBlob25DID) freeBlob25DList.Add(removeBlob);
                                    }
                                    break;

                                case TuioBlob25D.TUIO_ADDED:
                                    TuioBlob25D addBlob;
                                    lock (blob25DSync)
                                    {
                                        int b_id = blob25DList.Count;
                                        if ((blob25DList.Count <= maxBlob25DID) && (freeBlob25DList.Count > 0))
                                        {
                                            TuioBlob25D closestBlob = freeBlob25DList[0];
                                            IEnumerator<TuioBlob25D> testList = freeBlob25DList.GetEnumerator();
                                            while (testList.MoveNext())
                                            {
                                                TuioBlob25D testBlob = testList.Current;
                                                if (testBlob.getDistance(tblb) < closestBlob.getDistance(tblb)) closestBlob = testBlob;
                                            }
                                            b_id = closestBlob.BlobID;
                                            freeBlob25DList.Remove(closestBlob);
                                        }
                                        else maxBlob25DID = b_id;

                                        addBlob = new TuioBlob25D(currentTime, tblb.SessionID, b_id, tblb.X, tblb.Y, tblb.Z, tblb.Angle, tblb.Width, tblb.Height, tblb.Area);
                                        blob25DList.Add(addBlob.SessionID, addBlob);
                                    }

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioBlob25D(addBlob);
                                    }
                                    break;

                                default:
                                    TuioBlob25D updateBlob = getTuioBlob25D(tblb.SessionID);
                                    if ((tblb.X != updateBlob.X && tblb.XSpeed == 0) || (tblb.Y != updateBlob.Y && tblb.YSpeed == 0) || (tblb.Z != updateBlob.Z && tblb.ZSpeed == 0) || (tblb.Angle != updateBlob.Angle && tblb.RotationSpeed == 0))
                                        updateBlob.update(currentTime, tblb.X, tblb.Y, tblb.Z, tblb.Angle, tblb.Width, tblb.Height, tblb.Area);
                                    else
                                        updateBlob.update(currentTime, tblb.X, tblb.Y, tblb.Z, tblb.Angle, tblb.Width, tblb.Height, tblb.Area, tblb.XSpeed, tblb.YSpeed, tblb.ZSpeed, tblb.RotationSpeed, tblb.MotionAccel, tblb.RotationAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioBlob25D(updateBlob);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveBlob25DList;
                        aliveBlob25DList = newBlob25DList;
                        // recycling the List
                        newBlob25DList = buffer;
                    }
                    frameBlobs25D.Clear();
                }

            }
            else if (address == "/tuio/3Dobj")
            {
                if (command == "set")
                {

                    long s_id = (int)args[1];
                    int f_id = (int)args[2];
                    float xpos = (float)args[3];
                    float ypos = (float)args[4];
                    float zpos = (float)args[5];
                    float roll = (float)args[6];
                    float pitch = (float)args[7];
                    float yaw = (float)args[8];
                    float xspeed = (float)args[9];
                    float yspeed = (float)args[10];
                    float zspeed = (float)args[11];
                    float rollspeed = (float)args[12];
                    float pitchspeed = (float)args[13];
                    float yawspeed = (float)args[14];
                    float maccel = (float)args[15];
                    float raccel = (float)args[16];

                    lock (object3DSync)
                    {
                        if (!object3DList.ContainsKey(s_id))
                        {
                            TuioObject3D addObject = new TuioObject3D(s_id, f_id, xpos, ypos, zpos, roll, pitch, yaw);
                            frameObjects3D.Add(addObject);
                        }
                        else
                        {
                            TuioObject3D tobj = object3DList[s_id];
                            if (tobj == null) return;
                            if ((tobj.X != xpos) || (tobj.Y != ypos) || (tobj.Z != zpos) || (tobj.Roll != roll) || (tobj.Pitch != pitch) || (tobj.Yaw != yaw) || (tobj.XSpeed != xspeed) || (tobj.YSpeed != yspeed) || (tobj.ZSpeed != zspeed) || (tobj.RollSpeed != rollspeed) || (tobj.PitchSpeed != pitchspeed) || (tobj.YawSpeed != yawspeed) || (tobj.MotionAccel != maccel) || (tobj.RotationAccel != raccel))
                            {

                                TuioObject3D updateObject = new TuioObject3D(s_id, f_id, xpos, ypos, zpos, roll, pitch, yaw);
                                updateObject.update(xpos, ypos, zpos, roll, pitch, yaw, xspeed, yspeed, zspeed, rollspeed, pitchspeed, yawspeed, maccel, raccel);
                                frameObjects3D.Add(updateObject);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newObject3DList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newObject3DList.Add(s_id);
                        // reduce the object list to the lost objects
                        if (aliveObject3DList.Contains(s_id))
                            aliveObject3DList.Remove(s_id);
                    }

                    // remove the remaining objects
                    lock (object3DSync)
                    {
                        for (int i = 0; i < aliveObject3DList.Count; i++)
                        {
                            long s_id = aliveObject3DList[i];
                            TuioObject3D removeObject = object3DList[s_id];
                            removeObject.remove(currentTime);
                            frameObjects3D.Add(removeObject);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioObject3D> frameEnum = frameObjects3D.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioObject3D tobj = frameEnum.Current;

                            switch (tobj.TuioState)
                            {
                                case TuioObject3D.TUIO_REMOVED:
                                    TuioObject3D removeObject = tobj;
                                    removeObject.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioObject3D(removeObject);
                                    }
                                    lock (object3DSync)
                                    {
                                        object3DList.Remove(removeObject.SessionID);
                                    }
                                    break;
                                case TuioObject3D.TUIO_ADDED:
                                    TuioObject3D addObject = new TuioObject3D(currentTime, tobj.SessionID, tobj.SymbolID, tobj.X, tobj.Y, tobj.Z, tobj.Roll, tobj.Pitch, tobj.Yaw);
                                    lock (object3DSync)
                                    {
                                        object3DList.Add(addObject.SessionID, addObject);
                                    }
                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioObject3D(addObject);
                                    }
                                    break;
                                default:
                                    TuioObject3D updateObject = getTuioObject3D(tobj.SessionID);
                                    if ((tobj.X != updateObject.X && tobj.XSpeed == 0) || (tobj.Y != updateObject.Y && tobj.YSpeed == 0) || (tobj.Z != updateObject.Z && tobj.ZSpeed == 0) || (tobj.Roll != updateObject.Roll && tobj.RollSpeed == 0) || (tobj.Pitch != updateObject.Pitch && tobj.PitchSpeed == 0)|| (tobj.Yaw != updateObject.Yaw && tobj.YawSpeed == 0))
                                        updateObject.update(currentTime, tobj.X, tobj.Y, tobj.Z, tobj.Roll, tobj.Pitch, tobj.Yaw);
                                    else
                                        updateObject.update(currentTime, tobj.X, tobj.Y, tobj.Z, tobj.Roll, tobj.Pitch, tobj.Yaw, tobj.XSpeed, tobj.YSpeed, tobj.ZSpeed, tobj.RollSpeed, tobj.PitchSpeed, tobj.YawSpeed, tobj.MotionAccel, tobj.RotationAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioObject3D(updateObject);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveObject3DList;
                        aliveObject3DList = newObject3DList;
                        // recycling the List
                        newObject3DList = buffer;
                    }
                    frameObjects3D.Clear();
                }

            }
            else if (address == "/tuio/3Dcur")
            {

                if (command == "set")
                {

                    long s_id = (int)args[1];
                    float xpos = (float)args[2];
                    float ypos = (float)args[3];
                    float zpos = (float)args[4];
                    float xspeed = (float)args[5];
                    float yspeed = (float)args[6];
                    float zspeed = (float)args[7];
                    float maccel = (float)args[8];

                    lock (cursor3DList)
                    {
                        if (!cursor3DList.ContainsKey(s_id))
                        {

                            TuioCursor3D addCursor = new TuioCursor3D(s_id, -1, xpos, ypos, zpos);
                            frameCursors3D.Add(addCursor);

                        }
                        else
                        {
                            TuioCursor3D tcur = (TuioCursor3D)cursor3DList[s_id];
                            if (tcur == null) return;
                            if ((tcur.X != xpos) || (tcur.Y != ypos) || (tcur.Z != zpos) || (tcur.XSpeed != xspeed) || (tcur.YSpeed != yspeed) || (tcur.ZSpeed != zspeed) || (tcur.MotionAccel != maccel))
                            {
                                TuioCursor3D updateCursor = new TuioCursor3D(s_id, tcur.CursorID, xpos, ypos, zpos);
                                updateCursor.update(xpos, ypos, zpos, xspeed, yspeed, zspeed, maccel);
                                frameCursors3D.Add(updateCursor);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newCursor3DList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newCursor3DList.Add(s_id);
                        // reduce the cursor list to the lost cursors
                        if (aliveCursor3DList.Contains(s_id))
                            aliveCursor3DList.Remove(s_id);
                    }

                    // remove the remaining cursors
                    lock (cursor3DSync)
                    {
                        for (int i = 0; i < aliveCursor3DList.Count; i++)
                        {
                            long s_id = aliveCursor3DList[i];
                            if (!cursor3DList.ContainsKey(s_id)) continue;
                            TuioCursor3D removeCursor = cursor3DList[s_id];
                            removeCursor.remove(currentTime);
                            frameCursors3D.Add(removeCursor);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioCursor3D> frameEnum = frameCursors3D.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioCursor3D tcur = frameEnum.Current;
                            switch (tcur.TuioState)
                            {
                                case TuioCursor.TUIO_REMOVED:
                                    TuioCursor3D removeCursor = tcur;
                                    removeCursor.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioCursor3D(removeCursor);
                                    }
                                    lock (cursor3DSync)
                                    {
                                        cursor3DList.Remove(removeCursor.SessionID);

                                        if (removeCursor.CursorID == maxCursor3DID)
                                        {
                                            maxCursor3DID = -1;

                                            if (cursor3DList.Count > 0)
                                            {

                                                IEnumerator<KeyValuePair<long, TuioCursor3D>> clist = cursor3DList.GetEnumerator();
                                                while (clist.MoveNext())
                                                {
                                                    int f_id = clist.Current.Value.CursorID;
                                                    if (f_id > maxCursor3DID) maxCursor3DID = f_id;
                                                }

                                                List<TuioCursor3D> freeCursorBuffer = new List<TuioCursor3D>();
                                                IEnumerator<TuioCursor3D> flist = freeCursor3DList.GetEnumerator();
                                                while (flist.MoveNext())
                                                {
                                                    TuioCursor3D testCursor = flist.Current;
                                                    if (testCursor.CursorID < maxCursor3DID) freeCursorBuffer.Add(testCursor);
                                                }
                                                freeCursor3DList = freeCursorBuffer;
                                            }
                                            else freeCursor3DList.Clear();
                                        }
                                        else if (removeCursor.CursorID < maxCursor3DID) freeCursor3DList.Add(removeCursor);
                                    }
                                    break;

                                case TuioCursor.TUIO_ADDED:
                                    TuioCursor3D addCursor;
                                    lock (cursor3DSync)
                                    {
                                        int c_id = cursor3DList.Count;
                                        if ((cursor3DList.Count <= maxCursor3DID) && (freeCursor3DList.Count > 0))
                                        {
                                            TuioCursor3D closestCursor = freeCursor3DList[0];
                                            IEnumerator<TuioCursor3D> testList = freeCursor3DList.GetEnumerator();
                                            while (testList.MoveNext())
                                            {
                                                TuioCursor3D testCursor = testList.Current;
                                                if (testCursor.getDistance(tcur) < closestCursor.getDistance(tcur)) closestCursor = testCursor;
                                            }
                                            c_id = closestCursor.CursorID;
                                            freeCursor3DList.Remove(closestCursor);
                                        }
                                        else maxCursor3DID = c_id;

                                        addCursor = new TuioCursor3D(currentTime, tcur.SessionID, c_id, tcur.X, tcur.Y, tcur.Z);
                                        cursor3DList.Add(addCursor.SessionID, addCursor);
                                    }

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioCursor3D(addCursor);
                                    }
                                    break;

                                default:
                                    TuioCursor3D updateCursor = getTuioCursor3D(tcur.SessionID);
                                    if ((tcur.X != updateCursor.X && tcur.XSpeed == 0) || (tcur.Y != updateCursor.Y && tcur.YSpeed == 0) || (tcur.Z != updateCursor.Z && tcur.ZSpeed == 0))
                                        updateCursor.update(currentTime, tcur.X, tcur.Y, tcur.Z);
                                    else
                                        updateCursor.update(currentTime, tcur.X, tcur.Y, tcur.Z, tcur.XSpeed, tcur.YSpeed, tcur.ZSpeed, tcur.MotionAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioCursor3D(updateCursor);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveCursor3DList;
                        aliveCursor3DList = newCursor3DList;
                        // recycling the List
                        newCursor3DList = buffer;
                    }
                    frameCursors3D.Clear();
                }

            }
            else if (address == "/tuio/3Dblb")
            {

                if (command == "set")
                {

                    long s_id = (int)args[1];
                    float xpos = (float)args[2];
                    float ypos = (float)args[3];
                    float zpos = (float)args[4];
                    float roll = (float)args[5];
                    float pitch = (float)args[6];
                    float yaw = (float)args[7];
                    float width = (float)args[8];
                    float height = (float)args[9];
                    float depth = (float)args[10];
                    float volume = (float)args[11];
                    float xspeed = (float)args[12];
                    float yspeed = (float)args[13];
                    float zspeed = (float)args[14];
                    float rollspeed = (float)args[15];
                    float pitchspeed = (float)args[16];
                    float yawspeed = (float)args[17];
                    float maccel = (float)args[18];
                    float raccel = (float)args[19];

                    lock (blob3DList)
                    {
                        if (!blob3DList.ContainsKey(s_id))
                        {
                            TuioBlob3D addBlob = new TuioBlob3D(s_id, -1, xpos, ypos, zpos, roll, pitch, yaw, width, height, depth, volume);
                            frameBlobs3D.Add(addBlob);
                        }
                        else
                        {
                            TuioBlob3D tblb = (TuioBlob3D)blob3DList[s_id];
                            if (tblb == null) return;
                            if ((tblb.X != xpos) || (tblb.Y != ypos) || (tblb.Z != zpos) || (tblb.Roll != roll) || (tblb.Pitch != pitch) || (tblb.Yaw != yaw) || (tblb.Width != width) || (tblb.Height != height) || (tblb.Depth != depth) || (tblb.Volume != volume) || (tblb.XSpeed != xspeed) || (tblb.YSpeed != yspeed) || (tblb.ZSpeed != zspeed) || (tblb.RollSpeed != rollspeed) || (tblb.PitchSpeed != pitchspeed) || (tblb.YawSpeed != yawspeed) || (tblb.MotionAccel != maccel) || (tblb.RotationAccel != raccel))
                            {
                                TuioBlob3D updateBlob = new TuioBlob3D(s_id, tblb.BlobID, xpos, ypos, zpos, roll, pitch, yaw, width, height, depth, volume);
                                updateBlob.update(xpos, ypos, zpos, roll, pitch, yaw, width, height, depth, volume, xspeed, yspeed, zspeed, rollspeed, pitchspeed, yawspeed, maccel, raccel);
                                frameBlobs3D.Add(updateBlob);
                            }
                        }
                    }

                }
                else if (command == "alive")
                {

                    newBlob3DList.Clear();
                    for (int i = 1; i < args.Count; i++)
                    {
                        // get the message content
                        long s_id = (int)args[i];
                        newBlob3DList.Add(s_id);
                        // reduce the blob list to the lost blobs
                        if (aliveBlob3DList.Contains(s_id))
                            aliveBlob3DList.Remove(s_id);
                    }

                    // remove the remaining blobs
                    lock (blob3DSync)
                    {
                        for (int i = 0; i < aliveBlob3DList.Count; i++)
                        {
                            long s_id = aliveBlob3DList[i];
                            if (!blob3DList.ContainsKey(s_id)) continue;
                            TuioBlob3D removeBlob = blob3DList[s_id];
                            removeBlob.remove(currentTime);
                            frameBlobs3D.Add(removeBlob);
                        }
                    }

                }
                else if (command == "fseq")
                {
                    int fseq = (int)args[1];
                    bool lateFrame = false;

                    if (fseq > 0)
                    {
                        if (fseq > currentFrame) currentTime = TuioTime.SessionTime;
                        if ((fseq >= currentFrame) || ((currentFrame - fseq) > 100)) currentFrame = fseq;
                        else lateFrame = true;
                    }
                    else if ((TuioTime.SessionTime.TotalMilliseconds - currentTime.TotalMilliseconds) > 100)
                    {
                        currentTime = TuioTime.SessionTime;
                    }

                    if (!lateFrame)
                    {

                        IEnumerator<TuioBlob3D> frameEnum = frameBlobs3D.GetEnumerator();
                        while (frameEnum.MoveNext())
                        {
                            TuioBlob3D tblb = frameEnum.Current;
                            switch (tblb.TuioState)
                            {
                                case TuioBlob3D.TUIO_REMOVED:
                                    TuioBlob3D removeBlob = tblb;
                                    removeBlob.remove(currentTime);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.removeTuioBlob3D(removeBlob);
                                    }
                                    lock (blob3DSync)
                                    {
                                        blob3DList.Remove(removeBlob.SessionID);

                                        if (removeBlob.BlobID == maxBlob3DID)
                                        {
                                            maxBlob3DID = -1;

                                            if (blob3DList.Count > 0)
                                            {

                                                IEnumerator<KeyValuePair<long, TuioBlob3D>> blist = blob3DList.GetEnumerator();
                                                while (blist.MoveNext())
                                                {
                                                    int b_id = blist.Current.Value.BlobID;
                                                    if (b_id > maxBlob3DID) maxBlob3DID = b_id;
                                                }

                                                List<TuioBlob3D> freeBlobBuffer = new List<TuioBlob3D>();
                                                IEnumerator<TuioBlob3D> flist = freeBlob3DList.GetEnumerator();
                                                while (flist.MoveNext())
                                                {
                                                    TuioBlob3D testBlob = flist.Current;
                                                    if (testBlob.BlobID < maxBlob3DID) freeBlobBuffer.Add(testBlob);
                                                }
                                                freeBlob3DList = freeBlobBuffer;
                                            }
                                            else freeBlob3DList.Clear();
                                        }
                                        else if (removeBlob.BlobID < maxBlob3DID) freeBlob3DList.Add(removeBlob);
                                    }
                                    break;

                                case TuioBlob3D.TUIO_ADDED:
                                    TuioBlob3D addBlob;
                                    lock (blob3DSync)
                                    {
                                        int b_id = blob3DList.Count;
                                        if ((blob3DList.Count <= maxBlob3DID) && (freeBlob3DList.Count > 0))
                                        {
                                            TuioBlob3D closestBlob = freeBlob3DList[0];
                                            IEnumerator<TuioBlob3D> testList = freeBlob3DList.GetEnumerator();
                                            while (testList.MoveNext())
                                            {
                                                TuioBlob3D testBlob = testList.Current;
                                                if (testBlob.getDistance(tblb) < closestBlob.getDistance(tblb)) closestBlob = testBlob;
                                            }
                                            b_id = closestBlob.BlobID;
                                            freeBlob3DList.Remove(closestBlob);
                                        }
                                        else maxBlob3DID = b_id;

                                        addBlob = new TuioBlob3D(currentTime, tblb.SessionID, b_id, tblb.X, tblb.Y, tblb.Z, tblb.Roll, tblb.Pitch, tblb.Yaw, tblb.Width, tblb.Height, tblb.Depth,tblb.Volume);
                                        blob3DList.Add(addBlob.SessionID, addBlob);
                                    }

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.addTuioBlob3D(addBlob);
                                    }
                                    break;

                                default:
                                    TuioBlob3D updateBlob = getTuioBlob3D(tblb.SessionID);
                                    if ((tblb.X != updateBlob.X && tblb.XSpeed == 0) || (tblb.Y != updateBlob.Y && tblb.YSpeed == 0) || (tblb.Z != updateBlob.Z && tblb.ZSpeed == 0) || (tblb.Roll != updateBlob.Roll && tblb.RollSpeed == 0) || (tblb.Pitch != updateBlob.Pitch && tblb.PitchSpeed == 0) || (tblb.Yaw != updateBlob.Yaw && tblb.YawSpeed == 0))
                                        updateBlob.update(currentTime, tblb.X, tblb.Y, tblb.Z, tblb.Roll, tblb.Pitch, tblb.Yaw, tblb.Width, tblb.Height, tblb.Depth, tblb.Volume);
                                    else
                                        updateBlob.update(currentTime, tblb.X, tblb.Y, tblb.Z, tblb.Roll, tblb.Pitch, tblb.Yaw, tblb.Width, tblb.Height, tblb.Depth, tblb.Volume, tblb.XSpeed, tblb.YSpeed, tblb.ZSpeed, tblb.RollSpeed, tblb.PitchSpeed, tblb.YawSpeed, tblb.MotionAccel, tblb.RotationAccel);

                                    for (int i = 0; i < listenerList.Count; i++)
                                    {
                                        TuioListener listener = (TuioListener)listenerList[i];
                                        if (listener != null) listener.updateTuioBlob3D(updateBlob);
                                    }
                                    break;
                            }
                        }

                        for (int i = 0; i < listenerList.Count; i++)
                        {
                            TuioListener listener = (TuioListener)listenerList[i];
                            if (listener != null) listener.refresh(new TuioTime(currentTime));
                        }

                        List<long> buffer = aliveBlob3DList;
                        aliveBlob3DList = newBlob3DList;
                        // recycling the List
                        newBlob3DList = buffer;
                    }
                    frameBlobs3D.Clear();
                }

            }
        }

        #region Listener Management
        /**
		 * <summary>
         * Adds the provided TuioListener to the list of registered TUIO event listeners</summary>
         * <param name="listener">the TuioListener to add</param>
		 */
        public void addTuioListener(TuioListener listener)
        {
            listenerList.Add(listener);
        }

        /**
         * <summary>
         * Removes the provided TuioListener from the list of registered TUIO event listeners</summary>
         * <param name="listener">the TuioListener to remove</param>
         */
        public void removeTuioListener(TuioListener listener)
        {
            listenerList.Remove(listener);
        }

        /**
         * <summary>
         * Removes all TuioListener from the list of registered TUIO event listeners</summary>
         */
        public void removeAllTuioListeners()
        {
            listenerList.Clear();
        }
        #endregion

        #region Object Management

        /**
		 * <summary>
         * Returns a List of all currently active TuioObjects</summary>
         * <returns>a List of all currently active TuioObjects</returns>
		 */
        public List<TuioObject> getTuioObjects()
        {
            List<TuioObject> listBuffer;
            lock (objectSync)
            {
                listBuffer = new List<TuioObject>(objectList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns a List of all currently active TuioCursors</summary>
         * <returns>a List of all currently active TuioCursors</returns>
         */
        public List<TuioCursor> getTuioCursors()
        {
            List<TuioCursor> listBuffer;
            lock (cursorSync)
            {
                listBuffer = new List<TuioCursor>(cursorList.Values);
            }
            return listBuffer;
        }

		/**
         * <summary>
         * Returns a List of all currently active TuioBlobs</summary>
         * <returns>a List of all currently active TuioBlobs</returns>
         */
		public List<TuioBlob> getTuioBlobs()
		{
			List<TuioBlob> listBuffer;
			lock (blobSync)
			{
				listBuffer = new List<TuioBlob>(blobList.Values);
			}
			return listBuffer;
		}

        /**
         * <summary>
         * Returns the TuioObject corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioObject</summary>
         * <returns>an active TuioObject corresponding to the provided Session ID or NULL</returns>
         */
        public TuioObject getTuioObject(long s_id)
        {
            TuioObject tobject = null;
            lock (objectSync)
            {
                objectList.TryGetValue(s_id, out tobject);
            }
            return tobject;
        }

        /**
         * <summary>
         * Returns the TuioCursor corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioCursor</summary>
         * <returns>an active TuioCursor corresponding to the provided Session ID or NULL</returns>
         */
        public TuioCursor getTuioCursor(long s_id)
        {
            TuioCursor tcursor = null;
            lock (cursorSync)
            {
                cursorList.TryGetValue(s_id, out tcursor);
            }
            return tcursor;
        }

		/**
         * <summary>
         * Returns the TuioBlob corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioBlob</summary>
         * <returns>an active TuioBlob corresponding to the provided Session ID or NULL</returns>
         */
		public TuioBlob getTuioBlob(long s_id)
		{
			TuioBlob tblob = null;
			lock (blobSync)
			{
				blobList.TryGetValue(s_id, out tblob);
			}
			return tblob;
		}


        

        /**
         * <summary>
         * Returns a List of all currently active TuioObjects25D</summary>
         * <returns>a List of all currently active TuioObjects25D</returns>
         */
        public List<TuioObject25D> getTuioObjects25D()
        {
            List<TuioObject25D> listBuffer;
            lock (object25DSync)
            {
                listBuffer = new List<TuioObject25D>(object25DList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns a List of all currently active TuioCursors25D</summary>
         * <returns>a List of all currently active TuioCursors25D</returns>
         */
        public List<TuioCursor25D> getTuioCursors25D()
        {
            List<TuioCursor25D> listBuffer;
            lock (cursor25DSync)
            {
                listBuffer = new List<TuioCursor25D>(cursor25DList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns a List of all currently active TuioBlobs25D</summary>
         * <returns>a List of all currently active TuioBlobs25D</returns>
         */
        public List<TuioBlob25D> getTuioBlobs25D()
        {
            List<TuioBlob25D> listBuffer;
            lock (blob25DSync)
            {
                listBuffer = new List<TuioBlob25D>(blob25DList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns the TuioObject25D corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioObject25D</summary>
         * <returns>an active TuioObject25D corresponding to the provided Session ID or NULL</returns>
         */
        public TuioObject25D getTuioObject25D(long s_id)
        {
            TuioObject25D tobject = null;
            lock (object25DSync)
            {
                object25DList.TryGetValue(s_id, out tobject);
            }
            return tobject;
        }

        /**
         * <summary>
         * Returns the TuioCursor25D corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioCursor25D</summary>
         * <returns>an active TuioCursor25D corresponding to the provided Session ID or NULL</returns>
         */
        public TuioCursor25D getTuioCursor25D(long s_id)
        {
            TuioCursor25D tcursor = null;
            lock (cursor25DSync)
            {
                cursor25DList.TryGetValue(s_id, out tcursor);
            }
            return tcursor;
        }

        /**
         * <summary>
         * Returns the TuioBlob25D corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioBlob25D</summary>
         * <returns>an active TuioBlob25D corresponding to the provided Session ID or NULL</returns>
         */
        public TuioBlob25D getTuioBlob25D(long s_id)
        {
            TuioBlob25D tblob = null;
            lock (blob25DSync)
            {
                blob25DList.TryGetValue(s_id, out tblob);
            }
            return tblob;
        }






        /**
         * <summary>
         * Returns a List of all currently active TuioObjects3D</summary>
         * <returns>a List of all currently active TuioObjects3D</returns>
         */
        public List<TuioObject3D> getTuioObjects3D()
        {
            List<TuioObject3D> listBuffer;
            lock (object3DSync)
            {
                listBuffer = new List<TuioObject3D>(object3DList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns a List of all currently active TuioCursors3D</summary>
         * <returns>a List of all currently active TuioCursors3D</returns>
         */
        public List<TuioCursor3D> getTuioCursors3D()
        {
            List<TuioCursor3D> listBuffer;
            lock (cursor3DSync)
            {
                listBuffer = new List<TuioCursor3D>(cursor3DList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns a List of all currently active TuioBlobs3D</summary>
         * <returns>a List of all currently active TuioBlobs3D</returns>
         */
        public List<TuioBlob3D> getTuioBlobs3D()
        {
            List<TuioBlob3D> listBuffer;
            lock (blob3DSync)
            {
                listBuffer = new List<TuioBlob3D>(blob3DList.Values);
            }
            return listBuffer;
        }

        /**
         * <summary>
         * Returns the TuioObject3D corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioObject3D</summary>
         * <returns>an active TuioObject3D corresponding to the provided Session ID or NULL</returns>
         */
        public TuioObject3D getTuioObject3D(long s_id)
        {
            TuioObject3D tobject = null;
            lock (object3DSync)
            {
                object3DList.TryGetValue(s_id, out tobject);
            }
            return tobject;
        }

        /**
         * <summary>
         * Returns the TuioCursor3D corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioCursor3D</summary>
         * <returns>an active TuioCursor3D corresponding to the provided Session ID or NULL</returns>
         */
        public TuioCursor3D getTuioCursor3D(long s_id)
        {
            TuioCursor3D tcursor = null;
            lock (cursor3DSync)
            {
                cursor3DList.TryGetValue(s_id, out tcursor);
            }
            return tcursor;
        }

        /**
         * <summary>
         * Returns the TuioBlob3D corresponding to the provided Session ID
         * or NULL if the Session ID does not refer to an active TuioBlob3D</summary>
         * <returns>an active TuioBlob3D corresponding to the provided Session ID or NULL</returns>
         */
        public TuioBlob3D getTuioBlob3D(long s_id)
        {
            TuioBlob3D tblob = null;
            lock (blob3DSync)
            {
                blob3DList.TryGetValue(s_id, out tblob);
            }
            return tblob;
        }
















        #endregion

    }
}
