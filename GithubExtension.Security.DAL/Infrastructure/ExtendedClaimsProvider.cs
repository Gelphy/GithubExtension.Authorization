﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GithubExtension.Security.DAL.Entities;
using Claim = System.Security.Claims.Claim;

namespace GithubExtension.Security.DAL.Infrastructure
{
    public class ExtendedClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(User user)
        {

            List<Claim> claims = new List<Claim>();

            // apply needed logic for our claims

            //var daysInWork = (DateTime.Now.Date - user.JoinDate).TotalDays;

            //if (daysInWork > 90)
            //{
            //    claims.Add(CreateClaim("FTE", "1"));

            //}
            //else
            //{
            //    claims.Add(CreateClaim("FTE", "0"));
            //}

            //return claims;
            return null;
        }

        public static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }
    }
}
