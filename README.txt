TUIO C# LIBRARY AND EXAMPLES
----------------------------
Copyright (c) 2005-2016 Martin Kaltenbrunner <martin@tuio.org>
This software is part of reacTIVision, an open source fiducial
tracking and multi-touch framework based on computer vision. 
http://reactivision.sourceforge.net/

Many thanks to Andrew Jones <andrew.b.jones@gmail.com> for his 
improvements of the C# properties and documentation style.

Demo Applications:
------------------
This package contains two demo applications which are able
to receive TUIO messages from any TUIO enable tracker.

* TuioDump prints the TUIO events directly to the concole
* TuioDemo draws the object and cursor state on the screen

You can use these demo applications for debugging purposes, 
or use them as a starting point for the development of your own
C# applications implementing the TUIO protocol. Please refer to
the source code of the both examples and the following section.

Pressing F1 will toggle FullScreen mode with the TuioDemo,
pressing ESC or closing the Window will end the application.
Hitting the V key will print the TUIO events to the console.

Keep in mind to make your graphics scalable for the varying
screen and window resolutions. A reasonable TUIO application
will run in fullscreen mode, although the windowed mode might
be useful for debugging purposes or working with the Simulator.

For your convenience this example contains a monodevelop project.

Application Programming Interface:
----------------------------------
First you  need to create an instance of TuioClient. This class 
is listening to TUIO messages on the specified port and generates
higher level messages based on the object events.

Your application needs to implement the TuioListener interface,
and has to be added to the TuioClient in order to receive messages.

	"public class MyApplication : TuioListener"

A simple code snippet for setting up a TUIO session:

	MyApplication app = new MyApplication();
	TuioClient client = new TuioClient();
	client.addTuioListener(app);
	client.start();

A TuioListener needs to implement the following methods:

* addTuioObject(TuioObject tobj):
  this is called when an object becomes visible
* updateTuioObject(TuioObject tobj):
  and object was moved on the table surface
* removeTuioObject(TuioObject tobj):
  an object was removed from the table

* addTuioCursor(TuioCursor tcur):
  this is called when a new cursor is detected
* updateTuioCursor(TuioCursor tcur):
  a cursor is moving on the table surface
* removeTuioCursor(TuioCursor tcur):
  a cursor was removed from the table

* addTuioBlob(TuioBlob tblob):
  this is called when a new blob is detected
* updateTuiooBlob(TuioBlob tblob):
  a blob is moving on the table surface
* removeTuiooBlob(TuioBlob tblob):
  a blob was removed from the table

* refresh(TuioTime bundleTime):
  this method is called after each bundle,
  use it to repaint your screen for example

Each object or cursor is identified with a unique session ID that is maintained
over its lifetime. Additionally each object carries symbol ID that corresponds
to its attached fiducial marker number. The cursor ID of the cursor object is always
a number in the range of all currently detected cursor blobs.

The TuioObject and TuioCursor references are updated automatically by the TuioClient
and are always referencing to the same instance over the object lifetime.
All the TuioObject and TuioCursor attributes are encapsulated and can be
accessed with methods such as getX(), getY() and getAngle() and so on.
TuioObject and TuioCursor also have some additional convenience methods
for the calculation of distances and angles between objects. The getPath()
method returns a Vector of TuioPoints representing the movement path of the object.

Alternatively the TuioClient class contains some methods for the polling
of the currently visible objects and cursors. There are methods which return
either a list or individual object and cursor objects. The TuioObject and
TuioCursor classes have been added as a container which also can be used
by external classes.

* getTuioObjects() returns a Vector of all currently present TuioObjects
* getTuioCursors() returns a Vector of all currently present TuioCursors
* getTuioBlobs() returns a Vector of all currently present TuioBlobs
* getTuioObject(long s_id) returns a TuioObject (or NULL if not present)
* getTuioCursor(long s_id) returns a TuioCursor (or NULL if not present)
* getTuioBlob(long s_id) returns a TuioBlob (or NULL if not present)

License:
--------
This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3.0 of the License, or (at your option) any later version.
 
This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
Lesser General Public License for more details.
 
You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

References:
-----------
This example is using the OSC.NET OpenSound Control library for C#
along with a lot of changes and improvements by the author.
http://luvtechno.net/d/1980/02/open_sound_control_for_net_2.html
