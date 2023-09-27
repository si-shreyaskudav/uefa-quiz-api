using Gaming.Quiz.Contracts.Common;
using Gaming.Quiz.Contracts.Configuration;
using Gaming.Quiz.Contracts.Enums;
using Gaming.Quiz.Contracts.Sharing;
using Gaming.Quiz.Interfaces.Asset;
using Gaming.Quiz.Interfaces.Session;
using Gaming.Quiz.Interfaces.Sharing;
using Gaming.Quiz.Interfaces.Storage;
using Gaming.Quiz.Library.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaming.Quiz.Blanket.Sharing
{
    public class ImageGeneration : Common.BaseBlanket, IImageGenerationBlanket
    {
        private readonly IHostingEnvironment _Env;

        private static Config _ConfigData = null;
        private readonly String _Domain;
        private readonly String _BasePath;



        public ImageGeneration(IOptions<Application> appSettings, IAWS aws, IPostgre postgre, IRedis redis, ICookies cookies, IAsset asset,
          IHostingEnvironment env, IHttpContextAccessor httpContext)
            : base(appSettings, aws, postgre, redis, cookies, asset, httpContext)
        {
            _Env = env;
            _Domain = appSettings.Value.API.Domain;
            _BasePath = appSettings.Value.CustomSwaggerConfig.BasePath;
        }

        #region " Constant properties "

        private string _physicalPath { get { return _Env.ContentRootPath + "/"; } }
        private string _ConfigFile { get { return @"ImageSharing/Config/config_{0}.json"; } }
        private string _NoStreakImagePath { get { return @"ImageSharing/Images/share-bg1.jpg"; } }
        private string _StreakBaseImagePath { get { return @"ImageSharing/Images/share-bg2.jpg"; } }
        private string _StarFile { get { return @"ImageSharing/Images/Star.png"; } }
        private string _WrongFile { get { return @"ImageSharing/Images/Wrong.png"; } }
        private string _CorrectFile { get { return @"ImageSharing/Images/Correct.png"; } }
        private string _DashFile { get { return @"ImageSharing/Images/dash.png"; } }
        private string _StreakFile { get { return @"ImageSharing/Images/Streak.png"; } }



        private string _FontFile { get { return @"ImageSharing/Fonts/"; } }

        Dictionary<int, string> skillImageDict = new Dictionary<int, string>();

        #endregion " Constant properties "

        public async Task<HTTPResponse> GenerateImage(string GUID, string UserName, Int32 RightAns, Int32 TotalQtn, string StreakPts, Int32 TotPoints, List<Int32> CorrectAns, List<Int32> QsnStreak, string message, int platformId, string Lang, Int32 Percentile, int gamedayId, Int32 AnsPoints, string TimePoints)
        {
            HTTPResponse httpResponse = new HTTPResponse();
            HTTPMeta httpMeta = new HTTPMeta();
            ResponseObject responseObject = new ResponseObject();
            Stream imageStream = null;
            String imageExtension = "png";
            bool success = false;
            string error = string.Empty;
            String userMetaPath = String.Empty;
            Guid generatedGuid = Guid.NewGuid();
            GUID = generatedGuid.ToString();
            dynamic dData = new System.Dynamic.ExpandoObject();
            if (true)
            {
                try
                {
                    String fileName = GUID + "_" + gamedayId + "." + imageExtension;

                    String key = _Asset.ShareGraphics(fileName);//Location where user's Graphics are stored

                    imageStream = ImageProcessNew(RightAns, TotalQtn, StreakPts, TotPoints, CorrectAns, QsnStreak, message, Percentile, AnsPoints, TimePoints, ref error);
                    if (imageStream != null)
                    {
                        success = await _AWS.WriteImageOnS3(imageStream, key, imageExtension);
                        if (success)
                        {
                            success = WriteHtmlMeta(UserName, GUID, imageExtension, platformId, gamedayId, ref userMetaPath);
                            dData.MetaPath = userMetaPath;
                            responseObject.Value = dData;
                            responseObject.FeedTime = GenericFunctions.GetFeedTime();
                            httpResponse.Data = responseObject;
                        }
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-10, ref httpMeta, "Image not generated");
                        httpResponse.Data = -10;
                    }

                    if (success)
                    {
                        //GenericFunctions.AssetMeta(1, ref httpMeta, userMetaPath);
                        GenericFunctions.AssetMeta(1, ref httpMeta);
                        httpResponse.Data = dData;
                    }
                    else
                    {
                        GenericFunctions.AssetMeta(-20, ref httpMeta, error);
                        httpResponse.Data = -20;
                    }
                }
                catch (Exception ex)
                {
                    //HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Sharing.ImageGeneration.GenerateImage", ex.Message);
                    //_AWS.Log(httpLog);
                }
            }

            httpResponse.Meta = httpMeta;
            return httpResponse;
        }

        private Config GetConfig(int ClientId)
        {
            String mData = String.Empty;
            Config config = new Config();

            String physicalPath = "";
            physicalPath = _physicalPath + String.Format(_ConfigFile, ClientId);

            try
            {
                if (File.Exists(physicalPath))
                {
                    mData = File.ReadAllText(physicalPath);
                }
                else
                    throw new Exception("Config file does not exist.");
            }
            catch (Exception ex)
            {
                throw new Exception("Blanket.Sharing.ImageShare.GetConfig: " + ex.Message);
            }

            if (mData != String.Empty)
                config = GenericFunctions.Deserialize<Config>(mData);

            return config;
        }

        private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private Stream ImageProcessNew(Int32 RightAns, Int32 TotalQtn, string StreakPts, Int32 TotPoints, List<Int32> CorrectAns, List<Int32> QsnStreak, string message, Int32 Percentile, Int32 AnsPoints, string TimePoints, ref string error)
        {
            Stream imageStream = null;
            Config config;
            HTTPResponse imageResponse = new HTTPResponse();
            HTTPMeta imgMeta = new HTTPMeta();

            HTTPMeta httpMeta = new HTTPMeta();
            String GameDate = DateTime.Now.Date.ToString();
            String baseImagePath = string.Empty;
            try
            {
                int cId = 3;
                //config.txt
                config = GetConfig(cId);

                _ConfigData = config;

                //if (StreakCnt > 0)
                //    baseImagePath = _physicalPath + _StreakBaseImagePath;
                //else
                //    baseImagePath = _physicalPath + _NoStreakImagePath;

                baseImagePath = _physicalPath + _ConfigData.base_image.img_physical_path;
                Bitmap baseImage = (Bitmap)System.Drawing.Image.FromFile(baseImagePath);

                PositionTextOnImage(_ConfigData, ref baseImage, message, "Message", new Rectangle());
                PositionTextOnImage(_ConfigData, ref baseImage, TotPoints.ToString(), "TotalPoints", new Rectangle());
                PositionTextOnImage(_ConfigData, ref baseImage, AnsPoints.ToString(), "AnswerPoints", new Rectangle());
                PositionTextOnImage(_ConfigData, ref baseImage, $"{RightAns}/{TotalQtn}", "CorrectAnswer", new Rectangle());
                PositionTextOnImage(_ConfigData, ref baseImage, TimePoints.ToString(), "TimePoints", new Rectangle());
                PositionTextOnImage(_ConfigData, ref baseImage, StreakPts.ToString(), "StreakPoints", new Rectangle());

                using (Graphics g = Graphics.FromImage(baseImage))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    for (int i = 0; i < CorrectAns.Count; i++)
                    {
                        string imgName = CorrectAns[i] == 1 ? "correct" : CorrectAns[i] == 0 ? "wrong" : "notattempted";
                        string ballPath = String.Format(_ConfigData.ball_overlay.img_physical_path, imgName) + _ConfigData.ball_overlay.img_extension;
                        Bitmap ballImage = (Bitmap)System.Drawing.Image.FromFile(ballPath);
                        Coordinate c = _ConfigData.coordinates.Where(a => a.entity == "B" + (i + 1)).FirstOrDefault();
                        Rectangle ballImageRect = new Rectangle(c.xPos, c.yPos, c.width, c.height);

                        g.DrawRectangle(Pens.Transparent, Rectangle.Round(ballImageRect));
                        g.DrawImage(ballImage, ballImageRect);
                    }
                }

                //WriteProfile(RightAns, TotalQtn, StreakPts, TotPoints, CorrectAns, QsnStreak, message, Percentile, ref baseImage, false, true);

                //baseImage.Save("D:\\quiz.jpg", System.Drawing.Imaging.ImageFormat.Png);

                // QUALITY CONTROL
                System.Drawing.Imaging.ImageFormat imageFormat;
                if (_ConfigData.output_extension.ToLower() == ".jpg")
                    imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                else
                    imageFormat = System.Drawing.Imaging.ImageFormat.Png;

                System.Drawing.Imaging.ImageCodecInfo jpgEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Png);
                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                System.Drawing.Imaging.EncoderParameter myEncoderParameter = null;

                myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 85L);

                myEncoderParameters.Param[0] = myEncoderParameter;
                // END QUALITY CONTROL

                MemoryStream ms = new MemoryStream();
                baseImage.Save(ms, jpgEncoder, myEncoderParameters);
                //baseImage.Save(@$"D:\Swapnil\UAE Samples\sample_{cId}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                imageStream = ms;
                baseImage.Dispose();
            }
            catch (Exception ex)
            {
                error += ex.Message;
                HTTPLog httpLog = new HTTPLog();
                httpLog.Message = "Blanket.Sharing.ImageGeneration.ImageProcess" + ex.Message + ex.InnerException;
                _AWS.Log(httpLog);
            }

            return imageStream;
        }

        private void PositionTextOnImage(Config config, ref Bitmap baseImage, String mPrintText, String mEntityName, Rectangle rectangle, int ptsLen = 0)
        {
            using (Graphics g = Graphics.FromImage(baseImage))
            {
                Int32 mHeight = 0, mWidth = 0, mXPos = 0, mYPos = 0;
                FontDetail mFontProperty = config.font_details.FirstOrDefault(o => o.entity == mEntityName);
                Font mObjFont;
                SolidBrush mBrush;
                Coordinate crd = config.coordinates.FirstOrDefault(a => a.entity == mEntityName);
                //bool LengthGreater = ((mEntityName == "team_name" || mEntityName == "player_name") && mPrintText.Length > 13) || (mEntityName == "points" && mPrintText.Length > 2);
                //bool Le = mEntityName == "points" && mPrintText.Length ==1?;

                if (mEntityName.ToLower() == "message")
                {
                    if (mPrintText.Length <= 17)
                    {
                        mFontProperty.size = mFontProperty.size;
                    }
                    else if (mPrintText.Length > 17 && mPrintText.Length <= 18)
                    {
                        mFontProperty.size = (Convert.ToInt32(mFontProperty.size) - 3).ToString();
                    }
                    else if (mPrintText.Length >= 19 && mPrintText.Length <= 20)
                    {
                        mFontProperty.size = (Convert.ToInt32(mFontProperty.size) - 5).ToString();
                    }
                    else if (mPrintText.Length >= 21 && mPrintText.Length <= 22)
                    {
                        mFontProperty.size = (Convert.ToInt32(mFontProperty.size) - 7).ToString();
                    }
                    else
                    {
                        mFontProperty.size = (Convert.ToInt32(mFontProperty.size) - 10).ToString();
                    }

                }

                FontSetter(mFontProperty, FontStyle.Bold, out mObjFont, out mBrush);

                mXPos = crd.xPos;//ptsLen == 0 || ptsLen == 2 ? crd.xPos : ptsLen < 2 ? crd.xPos - Convert.ToInt32(mFontProperty.negativesize) : crd.xPos + Convert.ToInt32(mFontProperty.negativesize);
                mYPos = crd.yPos;
                mHeight = crd.height;



                // if config has width 0 thn use width of base image to centralise text. used for team name and match date
                mWidth = (crd.width == 0 ? baseImage.Width : crd.width);

                // if passing rectangle then passed rectangle else rectangle from config.txt
                rectangle = (rectangle == new Rectangle() ? new Rectangle(mXPos, mYPos, mWidth, mHeight) : rectangle);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                StringAlignment Al = StringAlignment.Center;
                StringAlignment LAl = StringAlignment.Center;

                if (mEntityName.ToLower() == "correctanswer" || mEntityName.ToLower() == "timepoints")
                {
                    Al = StringAlignment.Near;
                }
                else if (mEntityName.ToLower() == "answerpoints" || mEntityName.ToLower() == "streakpoints")
                {
                    Al = StringAlignment.Far;
                }

                using (var sf = new StringFormat()
                {
                    Alignment = Al,
                    LineAlignment = LAl,
                }) g.DrawString(mPrintText, mObjFont, mBrush, rectangle, sf);
            }
        }
        private void FontSetter(FontDetail fontProperty, FontStyle style, out Font font, out SolidBrush brush)
        {
            //Font Family
            //System.Drawing.FontFamily FontFamily = new System.Drawing.FontFamily(fontProperty.name);
            //System.Drawing.Font Font = new System.Drawing.Font(FontFamily, Int32.Parse(fontProperty.size), style, GraphicsUnit.Pixel);

            System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();

            String fontPath = _physicalPath + fontProperty.name;
            // String fontPath = _FontFile + fontProperty.name + ".ttf";

            pfc.AddFontFile(fontPath);

            System.Drawing.Font Font = new System.Drawing.Font(pfc.Families[0], float.Parse(fontProperty.size), style, GraphicsUnit.Pixel);

            //Font color
            //Color Color = System.Drawing.ColorTranslator.FromHtml(fontProperty.color);
            Color Color = Color.FromArgb(fontProperty.Red, fontProperty.Green, fontProperty.Blue);
            SolidBrush Brush = new SolidBrush(Color);

            font = Font;
            brush = Brush;
        }

        private bool WriteHtmlMeta(string username, string guid, String imageExtension, int plaformId, long gamedayId, ref String userMetaPath)
        {
            bool success = false;
            try
            {
                String fileName = $"{guid}" + "_" + gamedayId + "." + imageExtension;

                String path = _Asset.UserMetaPath(guid, guid);
                //userMetaPath = path;

                //userMetaPath = userMetaPath.Replace("/feeds/share/graphic/", "https://" + _Domain + _BasePath + "/feeds/share/graphic/").Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/");

                userMetaPath = String.Format(_ConfigData.metafile, guid);

                Guid generatedGuid = Guid.NewGuid();
                String imagePath = String.Format(_ConfigData.imagefile, fileName, generatedGuid);//$"https://{_Domain + _BasePath}{_Asset.ShareGraphics(fileName)}?v={generatedGuid}";
                //imagePath = imagePath.Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/");
                string meta = _ConfigData.meta;//_Asset.GET(_Asset.MetaPath()).Result;

                //meta = meta.Replace("{{shareurl}}", imagePath.Replace(_Domain + _BasePath + "static", _Domain + _BasePath + "static-assets").Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/"));
                meta = meta.Replace("{{shareurl}}", imagePath);


                //if (meta.Contains(_Domain + _BasePath + "static"))
                //{
                //    if (!meta.Contains(_Domain + _BasePath + "static-assets"))
                //        meta = meta.Replace(_Domain + _BasePath + "static", _Domain + _BasePath + "static-assets").Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/");
                //}

                meta = meta.Replace("{{sharetitle}}", _ConfigData.sharetitle);
                meta = meta.Replace("{{sharedesc}}", _ConfigData.sharedesc);
                meta = meta.Replace("{{siteurl}}", _ConfigData.siteurl);

                //if (plaformId == 1)
                //{
                //    //meta = meta.Replace("{{siteurl}}", _Domain + _BasePath + "home").Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/");
                //    meta = meta.Replace("{{siteurl}}", _ConfigData.siteurl);
                //}
                //else if (plaformId == 2)
                //{
                //    meta = meta.Replace("{{siteurl}}", _Domain + _BasePath + "home").Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/");
                //}
                //else
                //{
                //    meta = meta.Replace("{{siteurl}}", _Domain + _BasePath + "home").Replace("/quiz//", "/quiz/").Replace("/games/", "/static-assets/");
                //}



                success = _AWS.WriteS3Asset(path, MimeType.Text, meta);


            }
            catch (Exception ex)
            {
                HTTPLog httpLog = _Cookies.PopulateLog("Blanket.Share.ImageGenerate.WriteHtmlMeta", ex.Message);
                _AWS.Log(httpLog);
            }

            return success;
        }

        //private void FontSetter(FontDetail fontProperty, FontStyle style, out Font font, out SolidBrush brush)
        //{
        //    fontProperty.Alpha = 1;

        //    System.Drawing.Text.PrivateFontCollection pfc = new System.Drawing.Text.PrivateFontCollection();

        //    String fontPath = _physicalPath + _FontFile + fontProperty.name + ".otf";

        //    pfc.AddFontFile(fontPath);

        //    System.Drawing.Font Font = new System.Drawing.Font(pfc.Families[0], Int32.Parse(fontProperty.size), style, GraphicsUnit.Pixel);

        //    //Font color
        //    Color Color = Color.FromArgb(fontProperty.Red, fontProperty.Green, fontProperty.Blue);
        //    SolidBrush Brush = new SolidBrush(Color);

        //    font = Font;
        //    brush = Brush;
        //}

        //private void WriteProfile(Int32 RightAns, Int32 TotalQtn, Int32 StreakCnt, Int32 TotPoints, List<Int32> CorrectAns, List<Int32> QsnStreak, string message, Int32 Percentile, ref Bitmap baseImage, bool issecondline = false, bool issingleline = false)
        //{
        //    try
        //    {
        //        Graphics graphics = Graphics.FromImage(baseImage);
        //        List<FontDetail> fontDetails = new List<FontDetail>();

        //        fontDetails = _ConfigData.font_details;

        //        //quality
        //        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        //        FontDetail fn_noStreak = fontDetails.Where(o => o.entity == "nostreak").FirstOrDefault();
        //        FontDetail fn_Streak = fontDetails.Where(o => o.entity == "streak").FirstOrDefault();
        //        FontDetail fn_Message = fontDetails.Where(o => o.entity == "message").FirstOrDefault();
        //        FontDetail fn_lowscore = fontDetails.Where(o => o.entity == "lowscore").FirstOrDefault();

        //        Font objFont;
        //        SolidBrush brush;

        //        string QuestionPrint = RightAns + "/" + TotalQtn;
        //        string ScorePrint = TotPoints.ToString();

        //        if (StreakCnt != 0)
        //        {
        //            Bitmap wrongImg = (Bitmap)System.Drawing.Image.FromFile(_physicalPath + _WrongFile);
        //            Bitmap correctImg = (Bitmap)System.Drawing.Image.FromFile(_physicalPath + _CorrectFile);
        //            Bitmap StreakImg = (Bitmap)System.Drawing.Image.FromFile(_physicalPath + _StreakFile);
        //            Bitmap dashImg = (Bitmap)System.Drawing.Image.FromFile(_physicalPath + _DashFile);

        //            List<Int32> streakIndex = new List<int>();
        //            Int32 counter = 0;

        //            FontSetter(fn_Streak, FontStyle.Regular, out objFont, out brush);
        //            graphics.DrawString(ScorePrint, objFont, brush, _ConfigData.StreakCardStats[0].xPos, _ConfigData.StreakCardStats[0].yPos);

        //            FontSetter(fn_Streak, FontStyle.Regular, out objFont, out brush);
        //            graphics.DrawString(QuestionPrint, objFont, brush, _ConfigData.StreakCardStats[1].xPos, _ConfigData.StreakCardStats[1].yPos);

        //            if (Percentile <= 10)
        //            {
        //                FontSetter(fn_lowscore, FontStyle.Regular, out objFont, out brush);
        //                graphics.DrawString(message, objFont, brush, _ConfigData.StreakCardStats[4].xPos, _ConfigData.StreakCardStats[4].yPos);
        //            }
        //            else
        //            {
        //                FontSetter(fn_Message, FontStyle.Regular, out objFont, out brush);
        //                graphics.DrawString(message, objFont, brush, _ConfigData.StreakCardStats[3].xPos, _ConfigData.StreakCardStats[3].yPos);
        //            }

        //            #region "streak Print"

        //            counter = 0;
        //            float streakwidth = 159.39f;

        //            for (int j = 0; j < CorrectAns.Count; j++)
        //            {
        //                float baseXPos = _ConfigData.StreakCardStats[2].xPos;
        //                float xSapce = counter == 0 ? 0 : 21.4f;
        //                xSapce = counter > 1 ? counter * 21.4f : xSapce;
        //                float nextXPos = counter * _ConfigData.StreakCardStats[2].width;

        //                float xPos = baseXPos + nextXPos + xSapce;

        //                if (CorrectAns[j] == 1)
        //                {
        //                    if (j <= 7)
        //                    {
        //                        if (CorrectAns[j] == 1 && CorrectAns[j + 1] == 1 && CorrectAns[j + 2] == 1)
        //                        {
        //                            graphics.DrawImage(StreakImg, xPos, _ConfigData.StreakCardStats[2].yPos, streakwidth, _ConfigData.StreakCardStats[2].height);
        //                            counter += 3;
        //                            j += 2;
        //                        }
        //                        else
        //                        {
        //                            graphics.DrawImage(correctImg, xPos, _ConfigData.StreakCardStats[2].yPos, _ConfigData.StreakCardStats[2].width, _ConfigData.StreakCardStats[2].height);
        //                            counter++;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        graphics.DrawImage(correctImg, xPos, _ConfigData.StreakCardStats[2].yPos, _ConfigData.StreakCardStats[2].width, _ConfigData.StreakCardStats[2].height);
        //                        counter++;
        //                    }
        //                }
        //                else
        //                {
        //                    graphics.DrawImage(wrongImg, xPos, _ConfigData.StreakCardStats[2].yPos, _ConfigData.StreakCardStats[2].width, _ConfigData.StreakCardStats[2].height);
        //                    counter++;

        //                }
        //            }

        //            #endregion
        //        }
        //        else
        //        {
        //            FontSetter(fn_noStreak, FontStyle.Regular, out objFont, out brush);
        //            graphics.DrawString(ScorePrint, objFont, brush, _ConfigData.NoStreakCardStats[0].xPos, _ConfigData.NoStreakCardStats[0].yPos);

        //            FontSetter(fn_noStreak, FontStyle.Regular, out objFont, out brush);
        //            graphics.DrawString(QuestionPrint, objFont, brush, _ConfigData.NoStreakCardStats[1].xPos, _ConfigData.NoStreakCardStats[1].yPos);

        //            if (Percentile <= 10)
        //            {
        //                FontSetter(fn_lowscore, FontStyle.Regular, out objFont, out brush);
        //                graphics.DrawString(message, objFont, brush, _ConfigData.NoStreakCardStats[3].xPos, _ConfigData.NoStreakCardStats[3].yPos);
        //            }
        //            else
        //            {

        //                FontSetter(fn_Message, FontStyle.Regular, out objFont, out brush);
        //                graphics.DrawString(message, objFont, brush, _ConfigData.NoStreakCardStats[2].xPos, _ConfigData.NoStreakCardStats[2].yPos);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error From Image Write Profile ==> " + ex.Message);
        //    }
        //}


    }
}
