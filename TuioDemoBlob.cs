/*
	TUIO C# Demo - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

using System;
using System.Drawing;
using TUIO;

	public class TuioDemoBlob : TuioBlob
	{

		SolidBrush blbBrush = new SolidBrush(Color.FromArgb(64,64,64));
		SolidBrush fntBrush = new SolidBrush(Color.White);
		Font font = new Font("Arial", 10.0f);

	public TuioDemoBlob (long s_id, int b_id, float xpos, float ypos, float angle, float width, float height, float area) : base(s_id,b_id,xpos,ypos,angle,width,height,area) {
		}

		public TuioDemoBlob (TuioBlob tblb) : base(tblb) {
		}

		public void paint(Graphics g) {

			int x = (int)(xpos*TuioDemo.width);
			int y = (int)(ypos*TuioDemo.height);
			float w = width*TuioDemo.width;
			float h = height*TuioDemo.height;

			g.TranslateTransform(x,y);
			g.RotateTransform((float)(angle/Math.PI*180.0f));
			g.TranslateTransform(-x,-y);

			//g.FillRectangle(blbBrush, new Rectangle(x-size/2,y-size/2,size,size));
			g.FillEllipse(blbBrush, x-w/2, y-h/2, w, h);


			g.TranslateTransform(x,y);
			g.RotateTransform(-1*(float)(angle/Math.PI*180.0f));
			g.TranslateTransform(-x,-y);

			g.DrawString(blob_id+"",font, fntBrush, new PointF(x,y));
		}

	}
