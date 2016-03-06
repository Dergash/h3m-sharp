﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using HMM3;

namespace HROE.Controllers
{
    public class MapsController : ApiController
    {

        public String Maps = System.Web.Hosting.HostingEnvironment.MapPath("~/Maps/");

        // [Route("{id}/name"])
        [Route("name")]
        public Map getName(String MapName = "[SoD - RoE] ConfluxHero.h3m")
        {
            Map Test = ParseMap();
            return Test;
        }

        private Map ParseMap()
        {
            byte[] Source = File.ReadAllBytes(Maps + "[SoD-RoE] ConfluxHero.h3m");
            return new Map(Source);
        }
        
    }
}