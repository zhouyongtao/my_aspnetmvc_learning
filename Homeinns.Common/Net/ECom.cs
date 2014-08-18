using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Homeinns.Common.Net
{
    public class ECom
    {
        public ECom(System.Web.HttpCookieCollection cookies, System.String hostName, System.String profileID)
        {
            _gsver = "1.1.0.7";
            _gscmd = "ecom";
            _gssrvid = profileID;
            System.String hashCodeString = "";
            if (hostName.StartsWith("www."))
            {
                string[] subHostNames = hostName.Split(new char[] { '.' }, 2);
                if (subHostNames.Length > 1)
                {
                    hashCodeString = _gssrvid + "_" + subHostNames[1];
                }
                else
                {
                    hashCodeString = _gssrvid + "_" + hostName;
                }
            }
            else
            {
                hashCodeString = _gssrvid + "_" + hostName;
            }
            System.String tempgscs = "";
            System.String tempgscu = "";
            System.String cookieSuffix = this.getHashCode(hashCodeString, false);
            if (cookies != null)
            {
                for (int i = 0; i < cookies.Count; i++)
                {
                    System.String cookieName = cookies[i].Name;
                    if (cookieName.Equals("_gscs_" + cookieSuffix))
                    {
                        try
                        {
                            tempgscs = System.Web.HttpUtility.UrlDecode(cookies[cookieName].Value, System.Text.Encoding.UTF8);
                        }
                        catch (System.Exception ex1)
                        {

                        }
                    }
                    if (cookieName.Equals("_gscu_" + cookieSuffix))
                    {
                        try
                        {
                            tempgscu = System.Web.HttpUtility.UrlDecode(cookies[cookieName].Value, System.Text.Encoding.UTF8);
                        }
                        catch (System.Exception ex2)
                        {

                        }
                    }
                }
                try
                {
                    _gssid = tempgscs.Split(new char[] { '|' })[0];
                    System.String[] gscuSubStrs = tempgscu.Split(new char[] { '|' });
                    if (gscuSubStrs.Length == 1)
                    {
                        _gsuid = gscuSubStrs[0];
                    }
                    else
                    {
                        _gsuid = gscuSubStrs[1];
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            if (_gssid == null || _gssid.Equals(""))
            {
                _gssid = this.getASessionID();
            }
            if (_gsuid == null || _gsuid.Equals(""))
            {
                _gsuid = this.getRandomNumber();
            }
            _gsltime = ((System.DateTime.Now - new System.DateTime(1970, 1, 1)).Ticks / 10000).ToString();
            _gstmzone = this.getTimeZone();
            _gsrd = this.getRandomNumber();
        }

        private System.String _gifHostName = "http://www.webdissector.com/recv/gs.gif";
        private System.Collections.Generic.List<System.String> _additionalGifHostName = new System.Collections.Generic.List<System.String>();
        private System.String _urlString = "";
        private System.String getString = "";

        private System.String _gsver = "";
        private System.String _gscmd = "";
        private System.String _gssrvid = "";
        private System.String _gsuid = "";
        private System.String _gssid = "";
        private System.String _gsltime = "";
        private System.String _gstmzone = "";
        private System.String _gsrd = "";
        private System.String _gsorderid = "";
        private double _gstotal = 0;
        private int _gsquan = 0;
        private System.String _gsuserid = "";
        private System.String _gsproducts = "";
        private System.String _gsisserver = "1";

        private bool totalDone = false;
        private System.Collections.Generic.List<System.String> gsproducts = new System.Collections.Generic.List<System.String>();

        public void setServiceUrl(System.String gifHostName)
        {
            _gifHostName = gifHostName;
        }

        public void addAdditionalServiceUrl(System.String newGifHostName)
        {
            if (this._additionalGifHostName != null)
            {
                if (!this._additionalGifHostName.Contains(newGifHostName) && newGifHostName != this._gifHostName)
                {
                    this._additionalGifHostName.Add(newGifHostName);
                }
            }
        }

        public void clearAdditionalServiceUrls()
        {
            if (this._additionalGifHostName != null)
            {
                this._additionalGifHostName.Clear();
            }
        }

        public void addOrder(System.String orderID, double totalPrice, System.String userID)
        {
            _gsorderid = orderID;
            _gsuserid = userID;
            if (totalPrice > 0)
            {
                this._gstotal = totalPrice;
                this.totalDone = true;
            }
        }

        public void addProduct(System.String orderID, System.String productName, System.String sku, double unitPrice, int quantity, System.String category)
        {
            if (orderID == _gsorderid)
            {
                this._gsquan = this._gsquan + quantity;
                if (this.totalDone == false)
                {
                    this._gstotal = this._gstotal + unitPrice * quantity;
                }
                try
                {
                    System.Text.StringBuilder product = new System.Text.StringBuilder();
                    product.Append(System.Web.HttpUtility.UrlEncode("orderid::"));
                    product.Append(System.Web.HttpUtility.UrlEncode(orderID));
                    product.Append(System.Web.HttpUtility.UrlEncode(",,"));
                    product.Append(System.Web.HttpUtility.UrlEncode("name::"));
                    product.Append(System.Web.HttpUtility.UrlEncode(productName));
                    product.Append(System.Web.HttpUtility.UrlEncode(",,"));
                    product.Append(System.Web.HttpUtility.UrlEncode("sku::"));
                    product.Append(System.Web.HttpUtility.UrlEncode(sku));
                    product.Append(System.Web.HttpUtility.UrlEncode(",,"));
                    product.Append(System.Web.HttpUtility.UrlEncode("quantity::"));
                    product.Append(quantity);
                    product.Append(System.Web.HttpUtility.UrlEncode(",,"));
                    product.Append(System.Web.HttpUtility.UrlEncode("unitprice::"));
                    product.Append(unitPrice);
                    product.Append(System.Web.HttpUtility.UrlEncode(",,"));
                    product.Append(System.Web.HttpUtility.UrlEncode("price::"));
                    product.Append(System.Web.HttpUtility.UrlEncode((quantity * unitPrice).ToString()));
                    product.Append(System.Web.HttpUtility.UrlEncode(",,"));
                    product.Append(System.Web.HttpUtility.UrlEncode("category::"));
                    product.Append(System.Web.HttpUtility.UrlEncode(category));
                    gsproducts.Add(product.ToString());
                }
                catch (System.Exception ex)
                {

                }
            }
        }


        public void trackECom()
        {
            if (!_gsorderid.Equals("") && _gsorderid != null)
            {
                this.formatUrlString();
                try
                {
                    //如果url超过2000则按照一个一个产品单独发送
                    //.com
                    this.getString = _gifHostName + "?" + _urlString;
                    if (getString.Length > 2000)
                    {
                        for (int i = 0; i < gsproducts.Count; i++)
                        {
                            getString = _gifHostName + "?" + getUrlStringForEachProduct(i);
                            SendRequest(this.getString);
                        }
                    }
                    else
                    {
                        SendRequest(this.getString);
                    }

                    //.cn以及其他的服务器
                    if (this._additionalGifHostName != null)
                    {
                        int maxCount = this._additionalGifHostName.Count;
                        for (int i = 0; i < maxCount; i++)
                        {
                            System.String currentGifHostName = this._additionalGifHostName[i];
                            System.String destinationUrl = currentGifHostName + "?" + _urlString;
                            if (destinationUrl.Length > 2000)
                            {
                                for (int j = 0; j < gsproducts.Count; j++)
                                {
                                    destinationUrl = currentGifHostName + "?" + getUrlStringForEachProduct(j);
                                    SendRequest(destinationUrl);
                                }
                            }
                            else
                            {
                                SendRequest(destinationUrl);
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {

                }
            }
        }

        private System.String getUrlStringForEachProduct(int index)
        {
            try
            {
                //不要用字符串加法
                System.Text.StringBuilder strbuf = new System.Text.StringBuilder();
                strbuf.Append("gsver=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsver));
                strbuf.Append("&gscmd=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gscmd));
                strbuf.Append("&gssrvid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gssrvid));
                strbuf.Append("&gsuid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsuid));
                strbuf.Append("&gssid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gssid));
                strbuf.Append("&gsltime=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsltime));
                strbuf.Append("&gstmzone=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gstmzone));
                strbuf.Append("&gsrd=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsrd));
                strbuf.Append("&gsorderid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsorderid));
                strbuf.Append("&gstotal=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gstotal.ToString()));
                strbuf.Append("&gsquan=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsquan.ToString()));
                strbuf.Append("&gsuserid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsuserid));
                if (gsproducts.Count > index)
                {
                    strbuf.Append("&gsproducts=");
                    //0.6.0.6版本不用二次编码
                    //strbuf.append(URLEncoder.encode(this._gsproducts,"UTF-8"));
                    strbuf.Append(gsproducts[index]);
                }
                strbuf.Append("&isserver=");
                strbuf.Append(_gsisserver);
                return strbuf.ToString();
            }
            catch (System.Exception e)
            {

            }
            return "";
        }

        private void formatUrlString()
        {
            this.formatGsProduct();
            try
            {
                //不要用字符串加法
                System.Text.StringBuilder strbuf = new System.Text.StringBuilder();
                strbuf.Append("gsver=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsver));
                strbuf.Append("&gscmd=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gscmd));
                strbuf.Append("&gssrvid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gssrvid));
                strbuf.Append("&gsuid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsuid));
                strbuf.Append("&gssid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gssid));
                strbuf.Append("&gsltime=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsltime));
                strbuf.Append("&gstmzone=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gstmzone));
                strbuf.Append("&gsrd=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsrd));
                strbuf.Append("&gsorderid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsorderid));
                strbuf.Append("&gstotal=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gstotal.ToString()));
                strbuf.Append("&gsquan=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsquan.ToString()));
                strbuf.Append("&gsuserid=");
                strbuf.Append(System.Web.HttpUtility.UrlEncode(this._gsuserid));
                if (!this._gsproducts.Equals(""))
                {
                    strbuf.Append("&gsproducts=");
                    //0.6.0.6版本不用二次编码
                    //strbuf.append(URLEncoder.encode(this._gsproducts,"UTF-8"));
                    strbuf.Append(this._gsproducts);
                }
                strbuf.Append("&isserver=");
                strbuf.Append(_gsisserver);
                _urlString = strbuf.ToString();
            }
            catch (System.Exception e)
            {

            }
        }

        private void formatGsProduct()
        {
            int i = 0;
            if (gsproducts.Count >= 1)
            {
                for (i = 0; i < gsproducts.Count - 1; i++)
                {
                    _gsproducts = _gsproducts + gsproducts[i] + "||";
                }
                _gsproducts = _gsproducts + gsproducts[gsproducts.Count - 1];
            }
        }

        private System.String getASessionID()
        {
            System.Random r = new System.Random(System.DateTime.Now.Millisecond);
            r.Next();
            int rint = r.Next(90000) + 10000;
            return ((System.DateTime.Now - new System.DateTime(1970, 1, 1)).Ticks / 10000).ToString() + rint.ToString();
        }

        private System.String getTimeZone()
        {
            return System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now).Hours.ToString();
        }

        private System.String getRandomNumber()
        {
            System.Random r = new System.Random(System.DateTime.Now.Millisecond);
            int rint = r.Next(90000) + 10000;
            return ((System.DateTime.Now - new System.DateTime(1970, 1, 1)).Ticks / 10000).ToString() + rint.ToString();
        }

        public void SendRequest(System.String destinationUrl)
        {
            try
            {
                //.com
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(destinationUrl);
                request.Timeout = 10000;
                request.BeginGetResponse(delegate(IAsyncResult ar)
                {
                    try
                    {
                        request.EndGetResponse(ar);
                    }
                    catch (System.Exception)
                    {
                    }
                }, null);
            }
            catch (System.Exception ex)
            {
            }
        }

        //get hash code
        //para = profileid+"_"+hostname
        private System.String getHashCode(System.String str, bool caseSensitive)
        {
            int hash = 1315423911;
            int ch = 0;
            if (!caseSensitive)
            {
                str = str.ToLower();
            }

            for (int i = str.Length - 1; i >= 0; i--)
            {
                ch = (int)(str[i]);
                hash ^= ((hash << 5) + ch + (hash >> 2));
            }
            return (hash & 0x7FFFFFFF).ToString();
        }
    }
}
