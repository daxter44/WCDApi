using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;
using WCDApi.DataBase.Entity;

namespace WCDApi.Worker.HTML
{
    public static class HTMLMenager
    {
        public static HTMLCompareResult HTMLDocumentCompare( HtmlDocument _oldWebPage,  HtmlDocument _newWebPage)
        {
            HTMLCompareResult result;
            if (_oldWebPage.DocumentNode != _newWebPage.DocumentNode)
            {

                 result = new HTMLCompareResult(HistoryItemType.WebHasChanges, "something on website changes");
                // HistoryChange(this, myArgs);
            }
            else
            {
                 result = new HTMLCompareResult(HistoryItemType.NothingChanges, "no changes on website " );
                // HistoryChange(this, myArgs);
            }
            return result;
        }
        public static HTMLCompareResult HTMLNodeCompare( HtmlNode _oldNode,  HtmlNode _newNode)
        {
            HTMLCompareResult result;
            if (_oldNode != null || _newNode != null)
            {
                if (_oldNode.OuterHtml != _newNode.OuterHtml)
                {
                    result = new HTMLCompareResult(HistoryItemType.WebHasChanges, "something on website changes");
                    // HistoryChange(this, myArgs);
                }
                else
                {
                     result = new HTMLCompareResult( HistoryItemType.NothingChanges, "no changes on website ");
                    // HistoryChange(this, myArgs);
                }
            }
            else
            {
                 result = new HTMLCompareResult(HistoryItemType.Error, "cant find element on Website");
                // HistoryChange(this, myArgs);
            }
            return result;
        }
        public static String GetHtmlPage(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader source = new StreamReader(response.GetResponseStream());
                return source.ReadToEnd();
            }
            catch (Exception ex)
            {
                // DetectorArgs myArgs = new DetectorArgs("I can't check page. No internet");
                // HistoryChange(this, myArgs);
                return ex.Message;
            }

        }

    }
}
