﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using ImageResizer;

namespace PhotoContest.Web.Infrastructure.Dropbox
{
    public static class UploadImages
    {
        internal static List<string> UploadImage(HttpPostedFileBase upload, bool isProfile)
        {
            var basePath = HostingEnvironment.ApplicationPhysicalPath;
            var path = new List<string>();
            var commonResizeSettings = new ResizeSettings("width=800;height=800;format=jpg;mode=max");
            var thumbsResizeSettings = new ResizeSettings("width=200;height=200;format=jpg;mode=max");

            if (isProfile)
            {
                commonResizeSettings = new ResizeSettings("width=400;height=400;format=jpg;mode=max");
            }

            using (Stream newFile = System.IO.File.Create(basePath + "\\temp\\temp.jpg"))
            {
                ImageJob i = new ImageJob();
                i.ResetSourceStream = true;
                i = new ImageJob(upload.InputStream, newFile, commonResizeSettings);
                i.CreateParentDirectory = false; //Auto-create the uploads directory.
                i.Build();
            }

            using (FileStream file = new FileStream(basePath + "\\temp\\temp.jpg", FileMode.Open))
            {
                path.Add(Dropbox.Upload(upload.FileName, file));
            }

            ImageBuilder.Current.Build(basePath + "\\temp\\temp.jpg", basePath + "\\temp\\temp.jpg", thumbsResizeSettings);

            using (FileStream file = new FileStream(basePath + "\\temp\\temp.jpg", FileMode.Open))
            {
                path.Add(Dropbox.Upload(upload.FileName, file, "Thumbnails"));

            }

            return path;
        }
    }
}