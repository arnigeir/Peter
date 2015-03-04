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
using System.Web.Services;
using System.Data;
using fastJSON;
using System.Collections;



/// <summary>
/// Summary description for Peter
/// </summary>
[WebService(Namespace = "http://peter.ags.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class Peter : System.Web.Services.WebService {

    public Peter () {
    }


    [WebMethod]
    public string GetApplications()
    {
        return DBAccess.GetApplications().ToJSONString();
    }
    [WebMethod]
    public string AddApplication(string appId)
    {
        return DBAccess.AddApplication(appId).ToJSONString();
    }
    [WebMethod]
    public string RemoveApplication(string appId)
    {
        return DBAccess.RemoveApplication(appId).ToJSONString();
    }

    [WebMethod]
    public string GetRoles(string appId)
    {
        return DBAccess.GetRoles(appId).ToJSONString();
    }
    [WebMethod]
    public string GetUsers(string appId)
    {
        return DBAccess.GetUsers(appId).ToJSONString();
    }
    [WebMethod]
    public string GetUserRoles(string appId, string username)
    {
        return DBAccess.GetUserRoles(appId, username).ToJSONString();
    }
    [WebMethod]
    public string AddRole(string appId, string role)
    {
        return DBAccess.AddRole(appId, role).ToJSONString();
    }
    [WebMethod]
    public string RemoveRole(string appId, string role)
    {
        return DBAccess.RemoveRole(appId, role).ToJSONString();
    }

    [WebMethod]
    public string AddUserRole(string appId, string username, string role)
    {
        return DBAccess.AddUserRole(appId, username, role).ToJSONString();
    }
    [WebMethod]
    public string RemoveUserRole(string appId, string username, string role)
    {
        return DBAccess.RemoveUserRole(appId, username, role).ToJSONString();
    }
    [WebMethod]
    public string RemoveUser(string appId, string username)
    {
        return DBAccess.RemoveUser(appId, username).ToJSONString();
    }
    [WebMethod]
    public string AddUser(string appId, string username)
    {
        return DBAccess.RemoveUser(appId, username).ToJSONString();
    }



    
}
