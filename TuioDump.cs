/*
	TUIO C# Example - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>

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
			Console.WriteLine("add obj "+tobj.SymbolID+" "+tobj.SessionID+" "+tobj.X+" "+tobj.Y+" "+tobj.Angle);
		}

		public void updateTuioObject(TuioObject tobj) {
			Console.WriteLine("set obj "+tobj.SymbolID+" "+tobj.SessionID+" "+tobj.X+" "+tobj.Y+" "+tobj.Angle+" "+tobj.MotionSpeed+" "+tobj.RotationSpeed+" "+tobj.MotionAccel+" "+tobj.RotationAccel);
		}

		public void removeTuioObject(TuioObject tobj) {
			Console.WriteLine("del obj "+tobj.SymbolID+" "+tobj.SessionID);
		}

		public void addTuioCursor(TuioCursor tcur) {
			Console.WriteLine("add cur "+tcur.CursorID + " ("+tcur.SessionID+") "+tcur.X+" "+tcur.Y);
		}

		public void updateTuioCursor(TuioCursor tcur) {
			Console.WriteLine("set cur "+tcur.CursorID + " ("+tcur.SessionID+") "+tcur.X+" "+tcur.Y+" "+tcur.MotionSpeed+" "+tcur.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor tcur) {
			Console.WriteLine("del cur "+tcur.CursorID + " ("+tcur.SessionID+")");
		}

		public void addTuioBlob(TuioBlob tblb) {
			Console.WriteLine("add blb "+tblb.BlobID + " ("+tblb.SessionID+") "+tblb.X+" "+tblb.Y+" "+tblb.Angle+" "+tblb.Width+" "+tblb.Height+" "+tblb.Area);
		}

		public void updateTuioBlob(TuioBlob tblb) {
			Console.WriteLine("set blb "+tblb.BlobID + " ("+tblb.SessionID+") "+tblb.X+" "+tblb.Y+" "+tblb.Angle+" "+tblb.Width+" "+tblb.Height+" "+tblb.Area+" "+tblb.MotionSpeed+" "+tblb.RotationSpeed+" "+tblb.MotionAccel+" "+tblb.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob tblb) {
			Console.WriteLine("del blb "+tblb.BlobID + " ("+tblb.SessionID+")");
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
