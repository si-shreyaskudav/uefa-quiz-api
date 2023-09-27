using System;
using System.Collections.Generic;
using System.Text;

namespace Gaming.Quiz.Contracts.Sharing
{
    public class ShareModel
    {
        public string GUID { get; set; }
        public Int32 GamedayId { get; set; }
        public Int32 RightAns { get; set; }
        public Int32 TotalQtn { get; set; }
        public Int32 TotPoints { get; set; }
        public Int32 AnsPoints { get; set; }
        public string TimePoints { get; set; }
        public string StreakPts { get; set; }
        public List<Int32> Answers { get; set; }
        public int PlatformId { get; set; }
        public string Message { get; set; }
        //public int ClientId { get; set; }
        //public Int32 Percentile { get; set; }
        //public string UserName { get; set; }
        //public string Lang { get; set; }
        //public List<Int32> QsnStreak { get; set; }
    }

    #region " TEMP "
    //public class Overlay
    //{
    //    public string img_physical_path { get; set; }
    //    public string img_extension { get; set; }
    //}

    //public class LanguageDetails
    //{
    //    public String lang { get; set; }
    //    public String title { get; set; }
    //    public String description { get; set; }
    //    public String sitename { get; set; }
    //}
    //public class FontDetail
    //{
    //    public string entity { get; set; }
    //    public string name { get; set; }
    //    public string size { get; set; }
    //    public string color { get; set; }
    //    public Int32 Red { get; set; }
    //    public Int32 Green { get; set; }
    //    public Int32 Blue { get; set; }
    //    public Int32 Alpha { get; set; }
    //    public Int32 xPos { get; set; }
    //    public Int32 yPos { get; set; }
    //    public Int32 height { get; set; }
    //    public Int32 width { get; set; }
    //}

    //public class Coordinate
    //{
    //    public string entity { get; set; }
    //    public float xPos { get; set; }
    //    public float yPos { get; set; }
    //    public float width { get; set; }
    //    public float height { get; set; }
    //}

    //public class Player
    //{
    //    public string entity { get; set; }
    //    public string font_style { get; set; }
    //    public string font_size { get; set; }
    //    public string font_color { get; set; }

    //}

    //public class Config
    //{
    //    public String output_extension { get; set; }
    //    public List<Coordinate> StreakCardStats { get; set; }
    //    public List<Coordinate> NoStreakCardStats { get; set; }
    //    public List<Coordinate> Streak { get; set; }

    //    public List<Coordinate> Score { get; set; }
    //    public List<FontDetail> font_details { get; set; }
    //}
    #endregion " TEMP "

    public class Config
    {
        public String output_extension { get; set; }
        public Overlay base_image { get; set; }
        public Overlay ball_overlay { get; set; }
        public Overlay player_overlay { get; set; }
        public Overlay box_overlay { get; set; }
        public Overlay point_box_overlay { get; set; }
        public Overlay captain_overlay { get; set; }
        public List<Coordinate> coordinates { get; set; }
        public List<PointCoordinate> points_coordinates { get; set; }
        public List<FontDetail> font_details { get; set; }
        public Increment increments { get; set; }
        public string meta { get; set; }
        public string sharetitle { get; set; }
        public string sharedesc { get; set; }
        public string metafile { get; set; }
        public string imagefile { get; set; }
        public string siteurl { get; set; }
    }

    public class Overlay
    {
        public string img_physical_path { get; set; }
        public string img_extension { get; set; }
    }

    public class Coordinate
    {
        public string entity { get; set; }
        public int yPos { get; set; }
        public int xPos { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int firstNameYAdjust { get; set; }
        public int lastNameYAdjust { get; set; }
        public int pointsYAdjust { get; set; }
        public int flagyPos { get; set; }
        public int flagxPos { get; set; }
        public int flagwidth { get; set; }
        public int flagheight { get; set; }
    }

    public class PointCoordinate
    {
        public string entity { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class FontDetail
    {
        public string entity { get; set; }
        public string name { get; set; }
        public string size { get; set; }
        public string negativesize { get; set; }
        public string color { get; set; }
        public Int32 Red { get; set; }
        public Int32 Green { get; set; }
        public Int32 Blue { get; set; }
        public Int32 xPos { get; set; }
        public Int32 yPos { get; set; }
        public Int32 height { get; set; }
        public Int32 width { get; set; }
    }

    public class Increment
    {
        public int captain_xPos { get; set; }
        public int captain_yPos { get; set; }
        public int label_yPos { get; set; }
        public int player_points_gap { get; set; }
        public int title_team_gap { get; set; }
        public int point_label_yPos { get; set; }
        public float player_points_adjustment { get; set; }

    }

}
