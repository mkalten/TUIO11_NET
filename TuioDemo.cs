/*
	TUIO C# Demo - part of the reacTIVision project
	Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>
	Modified by Bremard Nicolas <nicolas@bremard.fr> on 11/2022

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
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TUIO;

	public class TuioDemo : Form , TuioListener
	{
		private TuioClient client;
		private Dictionary<long,TuioObject> objectList;
		private Dictionary<long,TuioCursor> cursorList;
		private Dictionary<long,TuioBlob> blobList;
        private Dictionary<long, TuioObject25D> object25DList;
        private Dictionary<long, TuioCursor25D> cursor25DList;
        private Dictionary<long, TuioBlob25D> blob25DList;
        private Dictionary<long, TuioObject3D> object3DList;
        private Dictionary<long, TuioCursor3D> cursor3DList;
        private Dictionary<long, TuioBlob3D> blob3DList;

        public static int width, height;
		private int window_width =  640;
		private int window_height = 480;
		private int window_left = 0;
		private int window_top = 0;
		private int screen_width = Screen.PrimaryScreen.Bounds.Width;
		private int screen_height = Screen.PrimaryScreen.Bounds.Height;

		private bool fullscreen;
		private bool verbose;

		Font font = new Font("Arial", 10.0f);
		SolidBrush fntBrush = new SolidBrush(Color.White);
		SolidBrush bgrBrush = new SolidBrush(Color.FromArgb(0,0,64));
		SolidBrush curBrush = new SolidBrush(Color.FromArgb(192, 0, 192));
		SolidBrush objBrush = new SolidBrush(Color.FromArgb(64, 0, 0));
		SolidBrush blbBrush = new SolidBrush(Color.FromArgb(64, 64, 64));
		Pen curPen = new Pen(new SolidBrush(Color.Blue), 1);

		public TuioDemo(int port) {
		
			verbose = false;
			fullscreen = false;
			width = window_width;
			height = window_height;

			this.ClientSize = new System.Drawing.Size(width, height);
			this.Name = "TuioDemo";
			this.Text = "TuioDemo";
			
			this.Closing+=new CancelEventHandler(Form_Closing);
			this.KeyDown+=new KeyEventHandler(Form_KeyDown);

			this.SetStyle( ControlStyles.AllPaintingInWmPaint |
							ControlStyles.UserPaint |
							ControlStyles.DoubleBuffer, true);

			objectList = new Dictionary<long,TuioObject>(128);
			cursorList = new Dictionary<long,TuioCursor>(128);
			blobList   = new Dictionary<long,TuioBlob>(128);
        
            object25DList = new Dictionary<long, TuioObject25D>(128);
            cursor25DList = new Dictionary<long, TuioCursor25D>(128);
            blob25DList = new Dictionary<long, TuioBlob25D>(128);

            object3DList = new Dictionary<long, TuioObject3D>(128);
            cursor3DList = new Dictionary<long, TuioCursor3D>(128);
            blob3DList = new Dictionary<long, TuioBlob3D>(128);

            client = new TuioClient(port);
			client.addTuioListener(this);

			client.connect();
		}

		private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {

 			if ( e.KeyData == Keys.F1) {
	 			if (fullscreen == false) {

					width = screen_width;
					height = screen_height;

					window_left = this.Left;
					window_top = this.Top;

					this.FormBorderStyle = FormBorderStyle.None;
		 			this.Left = 0;
		 			this.Top = 0;
		 			this.Width = screen_width;
		 			this.Height = screen_height;

		 			fullscreen = true;
	 			} else {

					width = window_width;
					height = window_height;

		 			this.FormBorderStyle = FormBorderStyle.Sizable;
		 			this.Left = window_left;
		 			this.Top = window_top;
		 			this.Width = window_width;
		 			this.Height = window_height;

		 			fullscreen = false;
	 			}
 			} else if ( e.KeyData == Keys.Escape) {
				this.Close();

 			} else if ( e.KeyData == Keys.V ) {
 				verbose=!verbose;
 			}

 		}

		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			client.removeTuioListener(this);

			client.disconnect();
			System.Environment.Exit(0);
		}

		public void addTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Add(o.SessionID,o);
			}
        if (verbose) Console.WriteLine("add obj "+o.SymbolID+" ("+o.SessionID+") "+o.X+" "+o.Y+" "+o.Angle);
		}

		public void updateTuioObject(TuioObject o) {

			if (verbose) Console.WriteLine("set obj "+o.SymbolID+" "+o.SessionID+" "+o.X+" "+o.Y+" "+o.Angle+" "+o.MotionSpeed+" "+o.RotationSpeed+" "+o.MotionAccel+" "+o.RotationAccel);
		}

		public void removeTuioObject(TuioObject o) {
			lock(objectList) {
				objectList.Remove(o.SessionID);
			}
			if (verbose) Console.WriteLine("del obj "+o.SymbolID+" ("+o.SessionID+")");
		}

		public void addTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Add(c.SessionID,c);
			}
			if (verbose) Console.WriteLine("add cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y);
		}

		public void updateTuioCursor(TuioCursor c) {
			if (verbose) Console.WriteLine("set cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y+" "+c.MotionSpeed+" "+c.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor c) {
			lock(cursorList) {
				cursorList.Remove(c.SessionID);
			}
			if (verbose) Console.WriteLine("del cur "+c.CursorID + " ("+c.SessionID+")");
 		}

		public void addTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Add(b.SessionID,b);
			}
			if (verbose) Console.WriteLine("add blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area);
		}

		public void updateTuioBlob(TuioBlob b) {
		
			if (verbose) Console.WriteLine("set blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area+" "+b.MotionSpeed+" "+b.RotationSpeed+" "+b.MotionAccel+" "+b.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob b) {
			lock(blobList) {
				blobList.Remove(b.SessionID);
			}
			if (verbose) Console.WriteLine("del blb "+b.BlobID + " ("+b.SessionID+")");
		}








    public void addTuioObject25D(TuioObject25D tobj)
    {
        lock (object25DList)
        {
            object25DList.Add(tobj.SessionID, tobj);
        }
        if(verbose)
            Console.WriteLine("add 25Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Angle);
    }

    public void updateTuioObject25D(TuioObject25D tobj)
    {
        if (verbose)
            Console.WriteLine("set 25Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Angle + " " + tobj.XSpeed + " " + tobj.YSpeed + " " + tobj.ZSpeed + " " + tobj.RotationSpeed + " " + tobj.MotionAccel + " " + tobj.RotationAccel);
    }

    public void removeTuioObject25D(TuioObject25D tobj)
    {
        lock (object25DList)
        {
            object25DList.Remove(tobj.SessionID);
        }
        if (verbose)
            Console.WriteLine("del 25Dobj " + tobj.SessionID + " " + tobj.SymbolID);
    }

    public void addTuioCursor25D(TuioCursor25D tcur)
    {
        lock (cursor25DList)
        {
            cursor25DList.Add(tcur.SessionID, tcur);
        }
        if (verbose)
            Console.WriteLine("add 25Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z);
    }

    public void updateTuioCursor25D(TuioCursor25D tcur)
    {
        if (verbose)
            Console.WriteLine("set 25Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z + " " + tcur.XSpeed + " " + tcur.YSpeed + " " + tcur.ZSpeed + " " + tcur.MotionAccel);
    }

    public void removeTuioCursor25D(TuioCursor25D tcur)
    {
        lock (cursor25DList)
        {
            cursor25DList.Remove(tcur.SessionID);
        }
        if (verbose)
            Console.WriteLine("del 25Dcur " + tcur.SessionID + " (" + tcur.CursorID + ")");
    }

    public void addTuioBlob25D(TuioBlob25D tblb)
    {
        lock (blob25DList)
        {
            blob25DList.Add(tblb.SessionID, tblb);
        }
        if (verbose)
            Console.WriteLine("add 25Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Angle + " " + tblb.Width + " " + tblb.Height + " " + tblb.Area);
    }

    public void updateTuioBlob25D(TuioBlob25D tblb)
    {
        if (verbose)
            Console.WriteLine("set 25Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Angle + " " + tblb.Width + " " + tblb.Height + " " + tblb.Area + " " + tblb.XSpeed + " " + tblb.YSpeed + " " + tblb.ZSpeed + " " + tblb.RotationSpeed + " " + tblb.MotionAccel + " " + tblb.RotationAccel);
    }

    public void removeTuioBlob25D(TuioBlob25D tblb)
    {
        lock (blob25DList)
        {
            blob25DList.Remove(tblb.SessionID);
        }
        if (verbose)
            Console.WriteLine("del 25Dblb " + tblb.SessionID + " (" + tblb.BlobID + ")");
    }






    public void addTuioObject3D(TuioObject3D tobj)
    {
        lock (object3DList)
        {
            object3DList.Add(tobj.SessionID, tobj);
        }
        if (verbose)
            Console.WriteLine("add 3Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Roll + " " + tobj.Pitch + " " + tobj.Yaw);
    }

    public void updateTuioObject3D(TuioObject3D tobj)
    {
        if (verbose)
            Console.WriteLine("set 3Dobj " + tobj.SessionID + " " + tobj.SymbolID + " " + tobj.X + " " + tobj.Y + " " + tobj.Z + " " + tobj.Roll + " " + tobj.Pitch + " " + tobj.Yaw + " " + tobj.XSpeed + " " + tobj.YSpeed + " " + tobj.ZSpeed + " " + tobj.RollSpeed + " " + tobj.PitchSpeed + " " + tobj.YawSpeed + " " + tobj.MotionAccel + " " + tobj.RotationAccel);
    }

    public void removeTuioObject3D(TuioObject3D tobj)
    {
        lock (object3DList)
        {
            object3DList.Remove(tobj.SessionID);
        }
        if (verbose)
            Console.WriteLine("del 3Dobj " + tobj.SessionID + " " + tobj.SymbolID);
    }

    public void addTuioCursor3D(TuioCursor3D tcur)
    {
        lock (cursor3DList)
        {
            cursor3DList.Add(tcur.SessionID, tcur);
        }
        if (verbose)
            Console.WriteLine("add 3Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z);
    }

    public void updateTuioCursor3D(TuioCursor3D tcur)
    {
        if (verbose)
            Console.WriteLine("set 3Dcur " + tcur.SessionID + " (" + tcur.CursorID + ") " + tcur.X + " " + tcur.Y + " " + tcur.Z + " " + tcur.XSpeed + " " + tcur.YSpeed + " " + tcur.ZSpeed + " " + tcur.MotionAccel);
    }

    public void removeTuioCursor3D(TuioCursor3D tcur)
    {
        lock (cursor3DList)
        {
            cursor3DList.Remove(tcur.SessionID);
        }
        if (verbose)
            Console.WriteLine("del 3Dcur " + tcur.SessionID + " (" + tcur.CursorID + ")");
    }

    public void addTuioBlob3D(TuioBlob3D tblb)
    {
        lock (blob3DList)
        {
            blob3DList.Add(tblb.SessionID, tblb);
        }
        if (verbose)
            Console.WriteLine("add 3Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Roll + " " + tblb.Pitch + " " + tblb.Yaw + " " + tblb.Width + " " + tblb.Height + " " + tblb.Depth + " " + tblb.Volume);
    }

    public void updateTuioBlob3D(TuioBlob3D tblb)
    {
        if (verbose)
            Console.WriteLine("set 3Dblb " + tblb.SessionID + " (" + tblb.BlobID + ") " + tblb.X + " " + tblb.Y + " " + tblb.Z + " " + tblb.Roll + " " + tblb.Pitch + " " + tblb.Yaw + " " + tblb.Width + " " + tblb.Height + " " + tblb.Depth + " " + tblb.Volume + " " + tblb.XSpeed + " " + tblb.YSpeed + " " + tblb.ZSpeed + " " + tblb.RollSpeed + " " + tblb.PitchSpeed + " " + tblb.YawSpeed + " " + tblb.MotionAccel + " " + tblb.RotationAccel);
    }

    public void removeTuioBlob3D(TuioBlob3D tblb)
    {
        lock (blob3DList)
        {
            blob3DList.Remove(tblb.SessionID);
        }
        if (verbose)
            Console.WriteLine("del 3Dblb " + tblb.SessionID + " (" + tblb.BlobID + ")");
    }


   

    public void refresh(TuioTime frameTime) {
			Invalidate();
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Getting the graphics object
			Graphics g = pevent.Graphics;
			g.FillRectangle(bgrBrush, new Rectangle(0,0,width,height));

			// draw the cursor path
			if (cursorList.Count > 0) {
 			 lock(cursorList) {
			 foreach (TuioCursor tcur in cursorList.Values) {
					List<TuioPoint> path = tcur.Path;
					TuioPoint current_point = path[0];

					for (int i = 0; i < path.Count; i++) {
						TuioPoint next_point = path[i];
						g.DrawLine(curPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
						current_point = next_point;
					}
					g.FillEllipse(curBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
					g.DrawString("2Dcur "+tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
				}
			}
		 }

			// draw the objects
			if (objectList.Count > 0) {
 				lock(objectList) {
					foreach (TuioObject tobj in objectList.Values) {
						int ox = tobj.getScreenX(width);
						int oy = tobj.getScreenY(height);
						int size = height / 10;

						g.TranslateTransform(ox, oy);
						g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));

						g.TranslateTransform(ox, oy);
						g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-ox, -oy);

						g.DrawString("2Dobj " + tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
					}
				}
			}

			// draw the blobs
			if (blobList.Count > 0) {
				lock(blobList) {
					foreach (TuioBlob tblb in blobList.Values) {
						int bx = tblb.getScreenX(width);
						int by = tblb.getScreenY(height);
						float bw = tblb.Width*width;
						float bh = tblb.Height*height;

						g.TranslateTransform(bx, by);
						g.RotateTransform((float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);

						g.FillEllipse(blbBrush, bx - bw / 2, by - bh / 2, bw, bh);

						g.TranslateTransform(bx, by);
						g.RotateTransform(-1 * (float)(tblb.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-bx, -by);
						
						g.DrawString("2Dblb " +
                            "" + tblb.BlobID + "", font, fntBrush, new PointF(bx, by));
					}
				}
			}

            // draw the 25Dcursor path
            if (cursor25DList.Count > 0)
            {
                lock (cursor25DList)
                {
                    foreach (TuioCursor25D tcur in cursor25DList.Values)
                    {
                        List<TuioPoint> path = tcur.Path;
                        TuioPoint current_point = path[0];

                        for (int i = 0; i < path.Count; i++)
                        {
                            TuioPoint next_point = path[i];
                            g.DrawLine(curPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
                            current_point = next_point;
                        }
                        g.FillEllipse(curBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
                        g.DrawString("25Dcur " + tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
                    }
                }
            }

            // draw the 25Dobjects
            if (object25DList.Count > 0)
            {
                lock (object25DList)
                {
                    foreach (TuioObject25D tobj in object25DList.Values)
                    {
                        int ox = tobj.getScreenX(width);
                        int oy = tobj.getScreenY(height);
                        int size = height / 10;

                        g.TranslateTransform(ox, oy);
                        g.RotateTransform((float)(tobj.Angle / Math.PI * 180.0f));
                        g.TranslateTransform(-ox, -oy);

                        g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));

                        g.TranslateTransform(ox, oy);
                        g.RotateTransform(-1 * (float)(tobj.Angle / Math.PI * 180.0f));
                        g.TranslateTransform(-ox, -oy);

                        g.DrawString("25Dobj " + tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
                    }
                }
            }

            // draw the 25Dblobs
            if (blob25DList.Count > 0)
            {
                lock (blob25DList)
                {
                    foreach (TuioBlob25D tblb in blob25DList.Values)
                    {
                        int bx = tblb.getScreenX(width);
                        int by = tblb.getScreenY(height);
                        float bw = tblb.Width * width;
                        float bh = tblb.Height * height;

                        g.TranslateTransform(bx, by);
                        g.RotateTransform((float)(tblb.Angle / Math.PI * 180.0f));
                        g.TranslateTransform(-bx, -by);

                        g.FillEllipse(blbBrush, bx - bw / 2, by - bh / 2, bw, bh);

                        g.TranslateTransform(bx, by);
                        g.RotateTransform(-1 * (float)(tblb.Angle / Math.PI * 180.0f));
                        g.TranslateTransform(-bx, -by);

                        g.DrawString("25Dblb " +
                            "" + tblb.BlobID + "", font, fntBrush, new PointF(bx, by));
                    }
                }
            }


            // draw the 3Dcursor path
            if (cursor3DList.Count > 0)
            {
                lock (cursor3DList)
                {
                    foreach (TuioCursor3D tcur in cursor3DList.Values)
                    {
                        List<TuioPoint> path = tcur.Path;
                        TuioPoint current_point = path[0];

                        for (int i = 0; i < path.Count; i++)
                        {
                            TuioPoint next_point = path[i];
                            g.DrawLine(curPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
                            current_point = next_point;
                        }
                        g.FillEllipse(curBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
                        g.DrawString("3Dcur " + tcur.CursorID + "", font, fntBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
                    }
                }
            }

            // draw the 3Dobjects
            if (object3DList.Count > 0)
            {
                lock (object3DList)
                {
                    foreach (TuioObject3D tobj in object3DList.Values)
                    {
                        int ox = tobj.getScreenX(width);
                        int oy = tobj.getScreenY(height);
                        int size = height / 10;

                        g.TranslateTransform(ox, oy);
                        g.RotateTransform((float)(tobj.Roll / Math.PI * 180.0f));
                        g.TranslateTransform(-ox, -oy);

                        g.FillRectangle(objBrush, new Rectangle(ox - size / 2, oy - size / 2, size, size));

                        g.TranslateTransform(ox, oy);
                        g.RotateTransform(-1 * (float)(tobj.Roll / Math.PI * 180.0f));
                        g.TranslateTransform(-ox, -oy);

                        g.DrawString("3Dobj " + tobj.SymbolID + "", font, fntBrush, new PointF(ox - 10, oy - 10));
                    }
                }
            }

            // draw the 3Dblobs
            if (blob3DList.Count > 0)
            {
                lock (blob3DList)
                {
                    foreach (TuioBlob3D tblb in blob3DList.Values)
                    {
                        int bx = tblb.getScreenX(width);
                        int by = tblb.getScreenY(height);
                        float bw = tblb.Width * width;
                        float bh = tblb.Height * height;

                        g.TranslateTransform(bx, by);
                        g.RotateTransform((float)(tblb.Roll / Math.PI * 180.0f));
                        g.TranslateTransform(-bx, -by);

                        g.FillEllipse(blbBrush, bx - bw / 2, by - bh / 2, bw, bh);

                        g.TranslateTransform(bx, by);
                        g.RotateTransform(-1 * (float)(tblb.Roll / Math.PI * 180.0f));
                        g.TranslateTransform(-bx, -by);

                        g.DrawString("3Dblb " +
                            "" + tblb.BlobID + "", font, fntBrush, new PointF(bx, by));
                    }
                }
            }



    }

    public static void Main(String[] argv) {
	 		int port = 0;
			switch (argv.Length) {
				case 1:
					port = int.Parse(argv[0],null);
					if(port==0) goto default;
					break;
				case 0:
					port = 3333;
					break;
				default:
					Console.WriteLine("usage: mono TuioDemo [port]");
					System.Environment.Exit(0);
					break;
			}
			
			TuioDemo app = new TuioDemo(port);
			Application.Run(app);
		}


}
