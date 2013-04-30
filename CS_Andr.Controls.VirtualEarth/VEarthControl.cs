using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace cs_Andr.Controls.VirtualEarth
{
    [System.Drawing.ToolboxBitmap(@"earth2.png")]
    [ProgId("cs_Andr.Controls.VirtualEarth")]
    [Guid("36814B19-4149-464a-9AB2-FA6A9100A7C4"), ClassInterface(ClassInterfaceType.AutoDispatch)]
    public partial class VEarthControl : UserControl
    {

        #region "Events declaration"

        public delegate void OnClickOnMapHandler(object sender, OnClickOnMapEventArgs e);
        public event OnClickOnMapHandler OnClickOnMap;

        public delegate void OnMoveOnMapHandler(object sender, OnMoveOnMapEventArgs e);
        public event OnMoveOnMapHandler OnMoveOnMap;

        public delegate void MapBusyErrorHandler(object sender, EventArgs e);
        public event MapBusyErrorHandler MapBusyError;

        #endregion

        #region "Current map configuration"

        //  2008/01  MapLoaded
        Boolean _MapIsLoaded = false;

        Int16 zoomLevel = 12;
        double lat = 39.753294;
        double lon = -105.115213;

        MapStyleEnum MapStyle = MapStyleEnum.Hybrid;
        DashboardStyleEnum DashBoardStyle = DashboardStyleEnum.Normal;

        public string getLon()
        {
            return lon.ToString();
        }

        public string getLat()
        {
            return lat.ToString();
        }


        public void GoToCoordinates(double Latit, double Longit)
        {
            lat = Latit;
            lon = Longit;

            ExecuteCommandOnMap("SetCenter", " new VELatLong( " + getLat() + "," + getLon() + ") ");
        }

        #endregion

        #region "Dashboard Management"

        public enum DashboardStyleEnum  {
            Small,
            Normal,
            Tiny
        }
        public void DashBoardShow()
        {
            ExecuteCommandOnMap("ShowDashboard");
        }

        public void DashBoardHide()
        {
            ExecuteCommandOnMap("HideDashboard");
        }



        public void DashBoardSet(DashboardStyleEnum dStyle)
        {
            //DashboardSmall = "VEDashboardSize.Small",
            //DashboardNormal = "VEDashboardSize.Normal",
            //DashboardTiny = "VEDashboardSize.Tiny"

            if (!((dStyle != DashboardStyleEnum.Small) &&
                (dStyle != DashboardStyleEnum.Normal) &&
                (dStyle != DashboardStyleEnum.Tiny)
                ))
            {
                String DBStyleString = DashboardStyleTransform(dStyle);

                DashBoardShow();
                DashBoardStyle = dStyle;
                ExecuteCommandOnMap("SetDashboardSize", "\"" + DBStyleString + "\"");
                ExecuteCommandOnMap("LoadMap");
                //webBrowser1.Navigate("javascript:map.SetMapStyle(\"" + mStyle + "\");");
            }
        }

        private static String DashboardStyleTransform(DashboardStyleEnum dStyle)
        {
            String DBStyleString = "VEDashboardSize.Normal";

            if (dStyle == DashboardStyleEnum.Small) DBStyleString = "VEDashboardSize.Small";
            if (dStyle == DashboardStyleEnum.Normal) DBStyleString = "VEDashboardSize.Normal";
            if (dStyle == DashboardStyleEnum.Tiny) DBStyleString = "VEDashboardSize.Tiny";
            return DBStyleString;
        }

        #endregion

        #region "MiniMap Management"
        public enum MiniMapStyle
        { 
            Small, Large
        }
        private Boolean MiniMapShowned = false;

        public void MiniMapShow()
        {
            ExecuteCommandOnMap("ShowMiniMap");
            MiniMapShowned = true;
        }
        public void MiniMapShow(MiniMapStyle style)
        {
            if(style== MiniMapStyle.Small)
                ExecuteCommandOnMap("ShowMiniMap", ",,VEMiniMapSize.Small");
            else
                ExecuteCommandOnMap("ShowMiniMap", ",,VEMiniMapSize.Large");

            MiniMapShowned = true;
        }

        public void MiniMapShow(Int32 left, Int32 top, MiniMapStyle style)
        {
            if (style == MiniMapStyle.Small)
                ExecuteCommandOnMap("ShowMiniMap", left.ToString() + "," + top.ToString() + ",VEMiniMapSize.Small");
            else
                ExecuteCommandOnMap("ShowMiniMap", left.ToString() + "," + top.ToString() + ",VEMiniMapSize.Large");

            MiniMapShowned = true;
        }


        public void MiniMapHide()
        {
            ExecuteCommandOnMap("HideMiniMap");

            MiniMapShowned = false;
        }

        public void MiniMapShowHide()
        {
            if (MiniMapShowned)
                MiniMapHide();
            else
                MiniMapShow();
        }

        #endregion

        #region "MapStyle Management"

        public enum MapStyleEnum  {
            Road ,
            Aerial ,
            Hybrid ,
            BirdsEye
        }

        public void SetMapStyle(MapStyleEnum mStyle)
        {

            //MapStyleRoad = "r",
            //MapStyleAerial = "a",
            //MapStyleHybrid = "h",
            //MapStyleBirdsEye = "o"

            if (!((mStyle != MapStyleEnum.Road) &&
                (mStyle != MapStyleEnum.Aerial) &&
                (mStyle != MapStyleEnum.Hybrid) &&
                (mStyle != MapStyleEnum.BirdsEye)
                ))
            {

                String mStyleString = MapStyleTransform(mStyle);

                MapStyle = mStyle;
                ExecuteCommandOnMap("SetMapStyle", "\"" + mStyleString + "\"");
                //webBrowser1.Navigate("javascript:map.SetMapStyle(\"" + mStyle + "\");");
            }
        }

        private String MapStyleTransform(MapStyleEnum mStyle)
        {
            String mStyleString = "r";

            if (mStyle == MapStyleEnum.Road) mStyleString = "r";
            if (mStyle == MapStyleEnum.Aerial) mStyleString = "a";
            if (mStyle == MapStyleEnum.Hybrid) mStyleString = "h";
            if (mStyle == MapStyleEnum.BirdsEye) mStyleString = "o";
            return mStyleString;
        }

        #endregion

        #region "Map is busy"
        private Boolean MapBusy = false;
        public Boolean MapBusyCheck = false;

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            MapBusy = false;
        }

        private bool MapIsBusy()
        {
            return MapBusy && MapBusyCheck;
        }

        #endregion

        #region "INIT and various, resize"

        public VEarthControl()
        {
            InitializeComponent();
        }

        private void VEarthControl_Resize(object sender, EventArgs e)
        {
            webBrowser1.Location = new Point(0, 0);
            webBrowser1.Size = this.Size;
        }

        public void unInit()
        {
            webBrowser1.Dispose();
            System.IO.File.Delete(System.IO.Path.GetTempPath() + "test.html");
        }

        private void VEarthControl_SizeChanged(object sender, EventArgs e)
        {
            ExecuteCommandOnMap("Resize", this.Width.ToString() + "," + this.Height.ToString());
            //webBrowser1.Navigate("javascript:map.Resize(" + this.Width.ToString() + "," + this.Height.ToString() + ");"); 
        }
        
        public String getIconAddress()
        {
            return "'http://80.204.79.87/kml-links/Cars/grey_XXX_green.png'";
        }

        #endregion

        #region "Zoom"
        public void SetZoomLevel(Int16 zLevel)
        {
            if ((1 <= zLevel) && (zLevel <= 19))
            {
                zoomLevel = zLevel;
                ExecuteCommand("SetZoomLevel", zoomLevel.ToString());
            }
        }

        public void ZoomOut()
        {
            SetZoomLevel((short)(zoomLevel - 1));
        }
        public void ZoomIn()
        {
            SetZoomLevel((short)(zoomLevel + 1));
        }

        public void ZoomBest()
        {
            SetZoomLevel(19);
        }

        public void ZoomWorld()
        {
            SetZoomLevel(1);
        }
        #endregion

        #region "MapLoading"

        private String MainHTMLFileName = "";
        public enum SDKVersion { Version4, Version5, Version6, }
        private SDKVersion ActualVersion = SDKVersion.Version6;
        
        public void ShowInitialMap(SDKVersion ver)
        {
            ShowInitialMap(ver, true);
        }

        public void ShowInitialMap(SDKVersion ver, Boolean AttachEvents)
        {
            ActualVersion = ver;
            String SDKVersionToUse = "";
            switch (ver)
            { 
                case SDKVersion.Version4:
                    SDKVersionToUse = "      <script src=\"http://dev.virtualearth.net/mapcontrol/v4/mapcontrol.js\"></script>\n";
                    break;
                case SDKVersion.Version5:
                    SDKVersionToUse = "      <script src=\"http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=5\"></script>\n";
                    break;
                case SDKVersion.Version6:
                    SDKVersionToUse = "      <script src=\"http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6\"></script>\n";
                    break;
            }
            

            String PageContent =
                    "<html>\n" +
                    "   <head>\n" +
                    "      <title></title>\n" +
                    "      <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n" +
                    SDKVersionToUse + 
                    "      <script>\n" +
                    "      var map = null;\n" +
                    "\n";

            PageContent +=
                    "    function SetZoomLevel(zLev)\n" +
                    "    {\n" +
                    "         map.SetZoomLevel(zLev);\n" +
                    "    }\n" +
                    "\n";

            PageContent +=
                    "    function myClickHandler(e)\n" +
                    "    {\n" +
                    "        var x = e.mapX;\n" +
                    "        var y = e.mapY;\n" +
                    "        var pixel = new VEPixel(x, y);\n" +
                    "        var LL = map.PixelToLatLong(pixel);\n" + 
                    "        if(e.leftMouseButton)\n" +
                    "            var events = \"snParameter: CLICKEVENT LEFT, \" + LL;\n" +
                    "        if(e.rightMouseButton)\n" +
                    "           var events = \"snParameter: CLICKEVENT RIGHT, \" + LL;\n" +
                  //"        window.navigate(events);\n" +
                    "        document.frames(\"external\").location=events;\n" +
                    "    }\n" +
                    "\n";

            PageContent +=
                    "    function myMouseMoveHandler(e)\n" +
                    "    {\n" +
                    "        var x = e.mapX;\n" +
                    "        var y = e.mapY;\n" +
                    "        var pixel = new VEPixel(x, y);\n" +
                    "        var LL = map.PixelToLatLong(pixel);\n" +

                    "        var events = \"snParameter: MOVEEVENT MOUSE, \" + LL;\n" +
                  //"        window.navigate(events);\n" +
                    "        document.frames(\"external\").location=events;\n" +
                    "    }\n" +
                    "\n";


            PageContent +=
                    "    function AddPin(pinID, posit, title, descr, iconAddress)\n" +
                    "    {   \n" +
                    "        var shape = new VEShape(VEShapeType.Pushpin, posit);\n" +
                    "        shape.SetTitle(title);\n" +
                    "        shape.SetDescription(descr);\n" +
 //                   "        pin.SetCustomIcon(iconAddress);\n" +
                    "           map.AddShape(shape);\n" +
                    "       \n" +
                    "    }\n" +
                    "\n";

        //    PageContent +=
        //"    function AddPin(pinID, posit, descr, iconAddress)\n" +
        //"    {   \n" +
        //"        var pin = new VEPushpin(\n" +
        //"        pinID, \n" +
        //"        posit, \n" +
        //"        iconAddress, \n" +
        //"        descr, \n" +
        //"        ''\n" +
        //"        );\n" +
        //"       pin.ShowDetailOnMouseOver=false;\n" +
        //"       map.AddPushpin(pin);\n" +
        //"    }\n" +
        //"\n";

            PageContent +=
                    "      function GetMap()\n" +
                    "      {\n" +
                    "         map = new VEMap('myMap');\n" +
                    "         map.LoadMap(new VELatLong( " + getLat() + "," + getLon() + "), 10 ,'h' , false);\n" +
                    "         map.SetZoomLevel(" + zoomLevel.ToString() + ");\n" +
                    "         map.SetMapStyle(\"" + MapStyleTransform(MapStyle) + "\");\n";


            if (AttachEvents)
            {
                PageContent +=
                    "         map.AttachEvent('onclick', myClickHandler); \n" +
                    "         map.AttachEvent('onmousemove', myMouseMoveHandler);\n";
            }
            
            PageContent +=
                    "      }   \n";
            PageContent +=
                    "      </script>\n" +
                    "   </head>\n" +
                    "   <body onload=\"GetMap();\"  style=\"margin-top:0; margin-left:0;\">\n" +
                    "      <div id='myMap' style=\"position:relative; width:" + this.Size.Width + "px; height:" + this.Size.Height + "px;\"></div>\n" +
                    "   </body>\n" +
                    "   <iframe id=\"external\" name=\"external\" frameborder=\"0\" src=\"\" width=\"0\" height=\"0\"><div>&nbsp;</div></iframe>\n" +
                    "</html>\n";

            //String fName = Application.StartupPath;
            //if (!fName.EndsWith("\\"))
            //    fName += "\\";
            
            //fName+="page.html";

            String fName = System.IO.Path.GetTempPath() + "test.html";
            MainHTMLFileName = fName;
            SavePage(PageContent, fName);

            webBrowser1.Navigate(fName);
            


            //  2008/01  MapLoaded
            DateTime mTimeToWait = DateTime.Now.AddSeconds(10);
            while ((webBrowser1.IsBusy) && (DateTime.Now < mTimeToWait))
            {
                Application.DoEvents();
            }

            //  2008/01  MapLoaded
            _MapIsLoaded = true;

        }

        private Boolean MapIsLoaded()
        {
            return _MapIsLoaded;
        }


        private void SavePage(string PageContent, string fName)
        {

            System.IO.StreamWriter x = new System.IO.StreamWriter(fName);
            x.Write(PageContent);
            x.Close();

        }

        #endregion

        #region "Pushpin"
        public void AddPushpin(Int32 IDPush, Double lat, Double lon, String Titolo, String Descrizione)
        {
            AddPushpin(IDPush, lat, lon, Titolo, Descrizione, getIconAddress());
        }

        public void AddPushpin(Int32 IDPush, Double lat, Double lon,String Titolo, String Descrizione, String iconAddress)
        {
            String Coordinate = "new VELatLong( " + lat.ToString().Replace(",", ".") + "," + lon.ToString().Replace(",", ".") + ")";
            String parametri = IDPush.ToString() + ", " 
                             + Coordinate + ", "
                             + "'" + Titolo.Replace("'", "\\'") + "', "
                             + "'" + Descrizione.Replace("'", "\\'") + "',"
                             + " " + iconAddress + "";
            
            ExecuteCommand("AddPin", parametri);

            // 2008/01
            //Attendo un Po???? perchè caaaaa nn me li fa vedere?
            DateTime AttendTo = DateTime.Now.AddMilliseconds(200);
            while (DateTime.Now<AttendTo) { Application.DoEvents(); }
        }

        public void RemovePushpin(Int32 IDPush)
        {
            if(ActualVersion == SDKVersion.Version4)
                ExecuteCommandOnMap("DeletePushpin", IDPush.ToString());
            else
                ExecuteCommandOnMap("DeleteShape", IDPush.ToString());
        }

        public void RemoveAllPushpins()
        {
            if (ActualVersion == SDKVersion.Version4)
                ExecuteCommandOnMap("DeleteAllPushpins");
            else
                ExecuteCommandOnMap("DeleteAllShapes");
        }
        #endregion

        #region "Commands on map"

        public void ExecuteCommandOnMap(String commandText)
        {
            ExecuteCommandOnMap(commandText,"");
        }

        public void ExecuteCommandOnMap(String commandText, String commandParameters)
        {
            // 2008/01 MapLoaded
            if ((!MapIsBusy() && MapIsLoaded()))
            {
                webBrowser1.Navigate("javascript:map." + commandText + "(" + commandParameters + ");");
                MapBusy = false;
            }
            else
                if(MapBusyError!=null)
                    MapBusyError(null, null);
        }

        public void ExecuteCommand(String commandText)
        {
            ExecuteCommand(commandText, "");
        }

        public void ExecuteCommand(String commandText, String commandParameters)
        { 
            // 2008/01 MapLoaded
            if ((!MapIsBusy() && MapIsLoaded()) && (!webBrowser1.IsDisposed))
            {
                webBrowser1.Navigate("javascript:" + commandText + "(" + commandParameters + ");");

                
                while (webBrowser1.IsBusy)
                    Application.DoEvents();

                MapBusy = false;
            }
            else
                if(MapBusyError!=null)
                    MapBusyError(null, null);

        }
        #endregion

        #region "Capture events from map"

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            MapBusy = true;

            String UrlNavTo = e.Url.ToString();

            if (UrlNavTo.StartsWith("snparameter:"))
            {
                MapBusy = false;
                e.Cancel = true;
                
                InterpreteOutCommand(UrlNavTo);
            }

            if (UrlNavTo.StartsWith("snparameter:") || UrlNavTo.StartsWith("javascript:"))
            {
                MapBusy = false;
            }
        }

        private void InterpreteOutCommand(string fileAddress)
        {
            
            //"snparameter: CLICKEVENT LEFT, 45.794579006904314, 9.245767593383796"
            fileAddress = fileAddress.Replace("snparameter: ","");
            
            MouseButtons mb = MouseButtons.None ;
            Double lat, lon;

            if (fileAddress.StartsWith("CLICKEVENT LEFT, "))
            {
                mb = MouseButtons.Left;
                fileAddress = fileAddress.Replace("CLICKEVENT LEFT, ", "");
            }

            if (fileAddress.StartsWith("CLICKEVENT RIGHT, "))
            {
                mb = MouseButtons.Right;
                fileAddress = fileAddress.Replace("CLICKEVENT RIGHT, ", "");
            }

            if (fileAddress.StartsWith("MOVEEVENT MOUSE, "))
            {
                mb = MouseButtons.None;
                fileAddress = fileAddress.Replace("MOVEEVENT MOUSE, ", "");
            }

            String[] a = fileAddress.Split(",".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            Double.TryParse(a[0], out lat);
            Double.TryParse(a[1], out lon);


            if (mb == MouseButtons.None)
            {
                OnMoveOnMapEventArgs clk = new OnMoveOnMapEventArgs(lat, lon);
                OnMoveOnMap(null, clk);
            
            }
            else
            {
                OnClickOnMapEventArgs clk = new OnClickOnMapEventArgs(lat, lon, mb);
                OnClickOnMap(null, clk);
            }
        }
        #endregion

        #region "Save and print map"

        public void PrintMapDefaultSettings()
        {
            webBrowser1.Print();
        }

        public void PrintMapPreview()
        {
            webBrowser1.ShowPrintPreviewDialog();
        }

        public void PrintOptions()
        {
            webBrowser1.ShowPrintDialog();
        }
#endregion

        #region "COM Component"
        [ComRegisterFunction]
        static void ComRegister(Type t)
        {
            string keyName = @"CLSID\" + t.GUID.ToString("B");
            using (RegistryKey key =
                     Registry.ClassesRoot.OpenSubKey(keyName, true))
            {
                key.CreateSubKey("Control").Close();
                using (RegistryKey subkey = key.CreateSubKey("MiscStatus"))
                {
                    subkey.SetValue("", "131457");
                }
                using (RegistryKey subkey = key.CreateSubKey("TypeLib"))
                {
                    Guid libid = Marshal.GetTypeLibGuidForAssembly(t.Assembly);
                    subkey.SetValue("", libid.ToString("B"));
                }
                using (RegistryKey subkey = key.CreateSubKey("Version"))
                {
                    Version ver = t.Assembly.GetName().Version;
                    string version =
                      string.Format("{0}.{1}",
                                    ver.Major,
                                    ver.Minor);
                    if (version == "0.0") version = "1.0";
                    subkey.SetValue("", version);
                }
            }
        }
        
        [ComUnregisterFunction]
        static void ComUnregister(Type t)
        {
            // Delete entire CLSID\{clsid} subtree
            string keyName = @"CLSID\" + t.GUID.ToString("B");
            Registry.ClassesRoot.DeleteSubKeyTree(keyName);
        }
        #endregion


        private void VEarthControl_Load(object sender, EventArgs e)
        {

        }
    }


    #region "Events arguments"
    public class OnClickOnMapEventArgs : EventArgs 
    {
        public Double Lat;
        public Double Lon;
        public MouseButtons MouseButton;
        public DateTime ora;

        public OnClickOnMapEventArgs(Double la, Double lo, MouseButtons mButt)
        { 
            Lat=la;
            Lon=lo;
            MouseButton = mButt;
            ora = DateTime.Now;
            //Console.WriteLine(ora.ToLongTimeString() + "." + ora.Millisecond);
        }
    }



    public class OnMoveOnMapEventArgs : EventArgs
    {
        public Double Lat;
        public Double Lon;
        public DateTime ora;

        public OnMoveOnMapEventArgs(Double la, Double lo)
        {
            Lat = la;
            Lon = lo;
            ora = DateTime.Now;
            //Console.WriteLine(ora.ToLongTimeString() + "." + ora.Millisecond);
        }
    }

    #endregion
}
