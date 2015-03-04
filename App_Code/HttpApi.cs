/*
Copyright (c) 2011 , Arni G Sigurdsson  (arni.geir.sigurdsson@gmail.is)

All rights reserved.

Redistribution of source or binary forms of software, 
with or without modification, is not permitted.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Net;
using System.IO;

/// <summary>
/// Summary description for HttpApi
/// </summary>
public static class HttpApi
{
    public static List<String> GetApplications()
    {
        return Call(CreateRequestURL("getapps", "", "", "")).Items;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public static List<String> GetUserRoles(String appId,String username)
    {
        return Call(CreateRequestURL("getuserroles", appId, username, "")).Items;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    public static List<String> GetUsers(String appId)
    {
        return Call(CreateRequestURL("getusers", appId, "", "")).Items;
    }

    //builds a url request string with parameters
    private static String CreateRequestURL(String action, String appId, String username, String role)
    {
        String url = System.Configuration.ConfigurationManager.AppSettings["RequestUrl"];
        url += String.Format("?action={0}&appid={1}&username={2}&role={3}", action, appId, username, role);
        return url;

    }

    private static QueryValue Call(String url)
    {
        // used to build entire input
        StringBuilder sb = new StringBuilder();

        // used on each read operation
        byte[] buf = new byte[8192];

        System.Net.HttpWebRequest request;
        Uri targetUri = new Uri(url);
        request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);


        // execute the request
        HttpWebResponse response = (HttpWebResponse)
            request.GetResponse();
        // we will read data via the response stream
        Stream resStream = response.GetResponseStream();
        string tempString = null;
        int count = 0;


        // fill the buffer with data
        count = resStream.Read(buf, 0, buf.Length);

        // make sure we read some data
        if (count != 0)
        {
            // translate from bytes to ASCII text
            tempString = Encoding.ASCII.GetString(buf, 0, count);

            // continue building the string
            sb.Append(tempString);
        }

        return QueryValue.Parse(sb.ToString());


    }
}