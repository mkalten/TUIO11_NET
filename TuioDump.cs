/*
	TUIO C# Example - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c)

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Intcur., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using TUIO;

	public class TuioDump : TuioListener
	{

		public void addTuioObject(TuioObject tobj) {
			Console.WriteLine("add obj "+tobj.SessionID+" " + tobj.SymbolID + " " + tobj.X+" "+tobj.Y+" "+tobj.Angle);
		}

		public void updateTuioObject(TuioObject tobj) {
			Console.WriteLine("set obj "+tobj.SessionID + " "+tobj.SymbolID + " "+tobj.X+" "+tobj.Y+" "+tobj.Angle + " " + tobj.XSpeed + " " + tobj.YSpeed + " "+tobj.RotationSpeed+" "+tobj.MotionAccel+" "+tobj.RotationAccel);
		}

		public void removeTuioObject(TuioObject tobj) {
			Console.WriteLine("del obj "+tobj.SessionID + " "+tobj.SymbolID);
		}

		public void addTuioCursor(TuioCursor tcur) {
			Console.WriteLine("add cur "+tcur.SessionID + " ("+tcur.CursorID + ") "+tcur.X+" "+tcur.Y);
		}

		public void updateTuioCursor(TuioCursor tcur) {
			Console.WriteLine("set cur "+tcur.SessionID + " ("+tcur.CursorID + ") "+tcur.X+" "+tcur.Y+" "+ tcur.XSpeed + " " + tcur.YSpeed + " "+tcur.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor tcur) {
			Console.WriteLine("del cur "+tcur.SessionID + " ("+tcur.CursorID + ")");
		}

		public void addTuioBlob(TuioBlob tblb) {
			Console.WriteLine("add blb "+tblb.SessionID + " ("+tblb.BlobID + ") "+tblb.X+" "+tblb.Y+" "+tblb.Angle+" "+tblb.Width+" "+tblb.Height+" "+tblb.Area);
		}

		public void updateTuioBlob(TuioBlob tblb) {
			Console.WriteLine("set blb "+tblb.SessionID + " ("+tblb.BlobID + ") "+tblb.X+" "+tblb.Y+" "+tblb.Angle+" "+tblb.Width+" "+tblb.Height+" "+tblb.Area+" "+ tblb.XSpeed + " " + tblb.YSpeed + " "+tblb.RotationSpeed+" "+tblb.MotionAccel+" "+tblb.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob tblb) {
			Console.WriteLine("del blb "+tblb.SessionID + " ("+tblb.BlobID + ")");
		}








        public void addTuioObject25D(TuioObject25D tobj)
        {
            Console.WriteLine("add 25Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Angle);
        }

        public void updateTuioObject25D(TuioObject25D tobj)
        {
            Console.WriteLine("set 25Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Angle + " " + tobj.XSpeed + " " + tobj.YSpeed + " " + tobj.ZSpeed + " " + tobj.RotationSpeed + " " + tobj.MotionAccel + " " + tobj.RotationAccel);
        }

        public void removeTuioObject25D(TuioObject25D tobj)
        {
            Console.WriteLine("del 25Dobj " + tobj.SessionID + " " + tobj.SymbolID);
        }

        public void addTuioCursor25D(TuioCursor25D tcur)
        {
            Console.WriteLine("add 25Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z);
        }

        public void updateTuioCursor25D(TuioCursor25D tcur)
        {
            Console.WriteLine("set 25Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z + " " + tcur.XSpeed + " " + tcur.YSpeed + " " + tcur.ZSpeed + " " + tcur.MotionAccel);
        }

        public void removeTuioCursor25D(TuioCursor25D tcur)
        {
            Console.WriteLine("del 25Dcur " + tcur.SessionID + " (" + tcur.CursorID + ")");
        }

        public void addTuioBlob25D(TuioBlob25D tblb)
        {
            Console.WriteLine("add 25Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Angle + " " + tblb.Width + " " + tblb.Height + " " + tblb.Area);
        }

        public void updateTuioBlob25D(TuioBlob25D tblb)
        {
            Console.WriteLine("set 25Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Angle + " " + tblb.Width + " " + tblb.Height + " " + tblb.Area + " " + tblb.XSpeed + " " + tblb.YSpeed + " " + tblb.ZSpeed + " " + tblb.RotationSpeed + " " + tblb.MotionAccel + " " + tblb.RotationAccel);
        }

        public void removeTuioBlob25D(TuioBlob25D tblb)
        {
            Console.WriteLine("del 25Dblb " + tblb.SessionID + " (" + tblb.BlobID + ")");
        }






            public void addTuioObject3D(TuioObject3D tobj)
            {
                Console.WriteLine("add 3Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Roll + " " + tobj.Pitch + " " + tobj.Yaw);
            }

            public void updateTuioObject3D(TuioObject3D tobj)
            {
                Console.WriteLine("set 3Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Roll + " " + tobj.Pitch + " " + tobj.Yaw + " " + tobj.XSpeed + " " + tobj.YSpeed + " " + tobj.ZSpeed + " " + tobj.RollSpeed + " " + tobj.PitchSpeed + " " + tobj.YawSpeed + " " + tobj.MotionAccel + " " + tobj.RotationAccel);
            }

            public void removeTuioObject3D(TuioObject3D tobj)
            {
                Console.WriteLine("del 3Dobj " + tobj.SessionID + " " + tobj.SymbolID);
            }

            public void addTuioCursor3D(TuioCursor3D tcur)
            {
                Console.WriteLine("add 3Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z);
            }

            public void updateTuioCursor3D(TuioCursor3D tcur)
            {
                Console.WriteLine("set 3Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z + " " + tcur.XSpeed + " " + tcur.YSpeed + " " + tcur.ZSpeed + " " + tcur.MotionAccel);
            }

            public void removeTuioCursor3D(TuioCursor3D tcur)
            {
                Console.WriteLine("del 3Dcur " + tcur.SessionID + " (" + tcur.CursorID + ")");
            }

            public void addTuioBlob3D(TuioBlob3D tblb)
            {
                Console.WriteLine("add 3Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Roll + " " + tblb.Pitch + " " + tblb.Yaw + " " + tblb.Width + " " + tblb.Height + " " + tblb.Depth + " " + tblb.Volume);
            }

            public void updateTuioBlob3D(TuioBlob3D tblb)
            {
                Console.WriteLine("set 3Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Roll + " " + tblb.Pitch + " " + tblb.Yaw + " " + tblb.Width + " " + tblb.Height + " " + tblb.Depth + " " + tblb.Volume + " " + tblb.XSpeed + " " + tblb.YSpeed + " " + tblb.ZSpeed + " " + tblb.RollSpeed + " " + tblb.PitchSpeed + " " + tblb.YawSpeed + " " + tblb.MotionAccel + " " + tblb.RotationAccel);
            }

            public void removeTuioBlob3D(TuioBlob3D tblb)
            {
                Console.WriteLine("del 3Dblb " + tblb.SessionID + " (" + tblb.BlobID + ")");
            }









    public void refresh(TuioTime frameTime) {
			//Console.WriteLine("refresh "+frameTime.getTotalMilliseconds());
		}

		public static void Main(String[] argv) {
			TuioDump demo = new TuioDump();
			TuioClient client = null;

			switch (argv.Length) {
				case 1:
					int port = 0;
					port = int.Parse(argv[0],null);
					if(port>0) client = new TuioClient(port);
					break;
				case 0:
					client = new TuioClient();
					break;
			}

			if (client!=null) {
				client.addTuioListener(demo);
				client.connect();
				Console.WriteLine("listening to TUIO messages at port " + client.getPort());

			} else Console.WriteLine("usage: mono TuioDump [port]");
		}
	}
