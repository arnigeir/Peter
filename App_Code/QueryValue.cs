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
using fastJSON;
using System.Collections;


/// <summary>
/// Summary description for ResultValue
/// </summary>
[Serializable()]
public class QueryValue
{
    private int status;
    private String message;
    private List<String> items;

    //getters and setters
    public int Status
    {
        get { return status; }
        set { status = value; }
    }
    public String Message
    {
        get { return message; }
        set { message = value; }
    }
    public List<String> Items
    {
        get { return items; }
        set { items = value; }
    }

    //constructors ..
    public QueryValue()
    {
        this.status = 1;
        this.message = "ok";
        this.items = new List<String>();
    }

    public QueryValue(int status, String message, List<String> items)
    {
        this.status = status;
        this.message = message;
        this.items = items;
    }

    //utility methods...

    //parses the string {status:'[0|1]',message:'text',items:['val1','val2',..]} into a ResultValue object
    public static QueryValue Parse(String jsonString)
    {
        QueryValue result = new QueryValue();
        object o = JSON.Instance.Parse(jsonString);
        Dictionary<String, object> d = o as Dictionary<String, object>;
        if (d != null)
        {
            if (d.ContainsKey("Status"))
            {
                String sVal = d["Status"] as String;
                if (sVal != null)
                {
                    int i = 0;
                    if (int.TryParse(sVal, out i)) result.Status = i;
                }
            }
            if (d.ContainsKey("Message")) result.Message = d["Message"] as String;
            if (d.ContainsKey("Items"))
            {
                ArrayList alist = d["Items"] as ArrayList;
                if(alist != null)
                    foreach (object obj in alist)
                        result.Items.Add(obj.ToString());
            }
        }

        return result;
    }
    //returns the JSON string of this object
    public string ToJSONString()
    {
        return JSON.Instance.ToJSON(this);
    }

}
