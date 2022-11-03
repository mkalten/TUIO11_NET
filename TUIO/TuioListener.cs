/*
 TUIO C# Library - part of the reacTIVision project
 Copyright (c) 

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
     * <para>
     * The TuioListener interface provides a simple callback infrastructure which is used by the {@link TuioClient} class
     * to dispatch TUIO events to all registered instances of classes that implement the TuioListener interface defined here.
     * </para>
     * <para>
     * Any class that implements the TuioListener interface is required to implement all of the callback methods defined here.
     * The {@link TuioClient} makes use of these interface methods in order to dispatch TUIO events to all registered TuioListener implementations.
     * </para>
     * <example>
     * <code>
     * public class MyTuioListener implements TuioListener
     * ...
     * MyTuioListener listener = new MyTuioListener();
     * TuioClient client = new TuioClient();
     * client.addTuioListener(listener);
     * client.start();
     * </code>
     * </example>
     *
     * @author Nicolas Bremard
     * @version 1.1.7
     */
    public interface TuioListener
    {
        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioObject is added to the session.</summary>
         *
         * <param name="tobj">the TuioObject reference associated to the addTuioObject event</param>
         */
        void addTuioObject(TuioObject tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioObject is updated during the session.</summary>
         *
         * <param name="tobj">the TuioObject reference associated to the updateTuioObject event</param>
         */
        void updateTuioObject(TuioObject tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioObject is removed from the session.</summary>
         *
         * <param name="tobj">the TuioObject reference associated to the removeTuioObject event</param>
         */
        void removeTuioObject(TuioObject tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioCursor is added to the session.</summary>
         *
         * <param name="tcur">the TuioCursor reference associated to the addTuioCursor event</param>
         */
        void addTuioCursor(TuioCursor tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioCursor is updated during the session.</summary>
         *
         * <param name="tcur">the TuioCursor reference associated to the updateTuioCursor event</param>
         */
        void updateTuioCursor(TuioCursor tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioCursor is removed from the session.</summary>
         *
         * <param name="tcur">the TuioCursor reference associated to the removeTuioCursor event</param>
         */
        void removeTuioCursor(TuioCursor tcur);

		/**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioBlob is added to the session.</summary>
         *
         * <param name="tblb">the TuioBlob reference associated to the addTuioBlob event</param>
         */
		void addTuioBlob(TuioBlob tblb);

		/**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioBlob is updated during the session.</summary>
         *
         * <param name="tblb">the TuioBlob reference associated to the updateTuioBlob event</param>
         */
		void updateTuioBlob(TuioBlob tblb);

		/**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioBlob is removed from the session.</summary>
         *
         * <param name="tblb">the TuioBlob reference associated to the removeTuioBlob event</param>
         */
		void removeTuioBlob(TuioBlob tblb);




        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioObject25D is added to the session.</summary>
         *
         * <param name="tobj">the TuioObject25D reference associated to the addTuioObject25D event</param>
         */
        void addTuioObject25D(TuioObject25D tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioObject25D is updated during the session.</summary>
         *
         * <param name="tobj">the TuioObject25D reference associated to the updateTuioObject25D event</param>
         */
        void updateTuioObject25D(TuioObject25D tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioObject25D is removed from the session.</summary>
         *
         * <param name="tobj">the TuioObject25D reference associated to the removeTuioObject25D event</param>
         */
        void removeTuioObject25D(TuioObject25D tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioCursor25D is added to the session.</summary>
         *
         * <param name="tcur">the TuioCursor25D reference associated to the addTuioCursor25D event</param>
         */
        void addTuioCursor25D(TuioCursor25D tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioCursor25D is updated during the session.</summary>
         *
         * <param name="tcur">the TuioCursor25D reference associated to the updateTuioCursor25D event</param>
         */
        void updateTuioCursor25D(TuioCursor25D tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioCursor25D is removed from the session.</summary>
         *
         * <param name="tcur">the TuioCursor25D reference associated to the removeTuioCursor25D event</param>
         */
        void removeTuioCursor25D(TuioCursor25D tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioBlob25D is added to the session.</summary>
         *
         * <param name="tblb">the TuioBlob25D reference associated to the addTuioBlob25D event</param>
         */
        void addTuioBlob25D(TuioBlob25D tblb);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioBlob25D is updated during the session.</summary>
         *
         * <param name="tblb">the TuioBlob25D reference associated to the updateTuioBlob25D event</param>
         */
        void updateTuioBlob25D(TuioBlob25D tblb);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioBlob25D is removed from the session.</summary>
         *
         * <param name="tblb">the TuioBlob25D reference associated to the removeTuioBlob25D event</param>
         */
        void removeTuioBlob25D(TuioBlob25D tblb);






        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioObject3D is added to the session.</summary>
         *
         * <param name="tobj">the TuioObject3D reference associated to the addTuioObject3D event</param>
         */
        void addTuioObject3D(TuioObject3D tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioObject3D is updated during the session.</summary>
         *
         * <param name="tobj">the TuioObject3D reference associated to the updateTuioObject3D event</param>
         */
        void updateTuioObject3D(TuioObject3D tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioObject3D is removed from the session.</summary>
         *
         * <param name="tobj">the TuioObject3D reference associated to the removeTuioObject3D event</param>
         */
        void removeTuioObject3D(TuioObject3D tobj);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioCursor3D is added to the session.</summary>
         *
         * <param name="tcur">the TuioCursor3D reference associated to the addTuioCursor3D event</param>
         */
        void addTuioCursor3D(TuioCursor3D tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioCursor3D is updated during the session.</summary>
         *
         * <param name="tcur">the TuioCursor3D reference associated to the updateTuioCursor3D event</param>
         */
        void updateTuioCursor3D(TuioCursor3D tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioCursor3D is removed from the session.</summary>
         *
         * <param name="tcur">the TuioCursor3D reference associated to the removeTuioCursor3D event</param>
         */
        void removeTuioCursor3D(TuioCursor3D tcur);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when a new TuioBlob3D is added to the session.</summary>
         *
         * <param name="tblb">the TuioBlob3D reference associated to the addTuioBlob3D event</param>
         */
        void addTuioBlob3D(TuioBlob3D tblb);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioBlob3D is updated during the session.</summary>
         *
         * <param name="tblb">the TuioBlob3D reference associated to the updateTuioBlob3D event</param>
         */
        void updateTuioBlob3D(TuioBlob3D tblb);

        /**
         * <summary>
         * This callback method is invoked by the TuioClient when an existing TuioBlob3D is removed from the session.</summary>
         *
         * <param name="tblb">the TuioBlob3D reference associated to the removeTuioBlob3D event</param>
         */
        void removeTuioBlob3D(TuioBlob3D tblb);









        /**
         * <summary>
         * This callback method is invoked by the TuioClient to mark the end of a received TUIO message bundle.</summary>
         *
         * <param name="ftime">the TuioTime associated to the current TUIO message bundle</param>
         */
        void refresh(TuioTime ftime);
    }
}
