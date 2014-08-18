using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace Homeinns.Common.Net
{

    public delegate void AsyncHttpCallback(AsyncHttp http, string content);
    public class RequestState
    {
        public const int BUFFER_SIZE = 1024;
        public byte[] BufferRead;
        public StringBuilder requestData;

        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;

        public AsyncHttpCallback cb;

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
            cb = null;
        }
    }

    public class AsyncHttp
    {
        public ManualResetEvent allDone = new ManualResetEvent(false);
        const int DefaultTimeout = 60 * 1000; // 1 minutes timeout

        //异步方式发起http get请求
        public bool HttpGet(string url, string queryString, AsyncHttpCallback callback)
        {
            if (!string.IsNullOrEmpty(queryString))
            {
                url += "?" + queryString;
            }

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.ServicePoint.Expect100Continue = false;

            try
            {
                RequestState state = new RequestState() { cb = callback, request = webRequest };
                IAsyncResult result = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
                // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), webRequest, DefaultTimeout, true);
                // The response came in the allowed time. The work processing will happen in the callback function.
                allDone.WaitOne();
                // Release the HttpWebResponse resource.
                state.response.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }


        //异步方式发起http post请求
        public bool HttpPost(string url, string queryString, AsyncHttpCallback callback)
        {
            StreamWriter requestWriter = null;
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ServicePoint.Expect100Continue = false;
                try
                {
                    //POST the data.
                    requestWriter = new StreamWriter(webRequest.GetRequestStream());
                    requestWriter.Write(queryString);
                    requestWriter.Close();
                    requestWriter = null;
                    var state = new RequestState() { cb = callback, request = webRequest };
                    var result = (IAsyncResult)webRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), state);
                    // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                    ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), webRequest, DefaultTimeout, true);

                    // The response came in the allowed time. The work processing will happen in the
                    // callback function.
                    allDone.WaitOne();
                    // Release the HttpWebResponse resource.
                    state.response.Close();
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (requestWriter != null)
                    {
                        requestWriter.Close();
                    }
                }
            }

            return true;
        }


        private void ResponseCallback(IAsyncResult asynchronousResult)
        {
            var state = (RequestState)asynchronousResult.AsyncState;
            try
            {
                HttpWebRequest webRequest = state.request;
                state.response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                Stream responseStream = state.response.GetResponseStream();
                state.streamResponse = responseStream;
                if (responseStream != null)
                {
                    IAsyncResult asynchronousInputRead = responseStream.BeginRead(state.BufferRead, 0, RequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), state);
                }
                return;
            }
            catch
            {
                FireCallback(state);
            }
            allDone.Set();
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            var state = (RequestState)asyncResult.AsyncState;
            Stream responseStream = state.streamResponse;
            try
            {
                int read = responseStream.EndRead(asyncResult);

                if (read > 0)
                {
                    state.requestData.Append(Encoding.UTF8.GetString(state.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(state.BufferRead, 0,
                        RequestState.BUFFER_SIZE, new AsyncCallback(ReadCallBack), state);

                    return;
                }
                else
                {
                    FireCallback(state);
                    responseStream.Close();
                }
            }
            catch
            {
                //fire back
                FireCallback(state);
                responseStream.Close();
            }

            allDone.Set();
        }

        // Abort the request if the timer fires.
        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                var request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        private void FireCallback(RequestState state)
        {
            //call back
            if (state.cb != null)
            {
                state.cb(this, state.requestData.ToString());
            }
        }
    }
}
