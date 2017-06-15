public void GetAllImages()
        {

            // Bing Image Result for Cat, First Page
            string url = "http://www.bing.com/images/search?q=cat&go=&form=QB&qs=n";
            string csFiles = "http://open-hardware-monitor.googlecode.com/svn/trunk/Hardware/";

            // For speed of dev, I use a WebClient
            WebClient client = new WebClient();
            string html = client.DownloadString(url);

            // Load the Html into the agility pack
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            // Now, using LINQ to get all Images
            /*List<HtmlNode> imageNodes = null;
            imageNodes = (from HtmlNode node in doc.DocumentNode.SelectNodes("//img")
                          where node.Name == "img"
                          && node.Attributes["class"] != null
                          && node.Attributes["class"].Value.StartsWith("sg_t")
                          select node).ToList();*/

           var imageLinks = doc.DocumentNode.Descendants("img")
    .Where(n => n.Attributes["class"].Value == "sg_t")
    .Select(n => HttpUtility.ParseQueryString(n.Attributes["src"].Value)["amp;url"]).ToList();


            foreach (string node in imageLinks)
            {
                y++;
                //Console.WriteLine(node.Attributes["src"].Value);
                richTextBox1.Text += node + Environment.NewLine;
                Image t = DownloadImage(node);
                t.Save(@"d:\test\" + y.ToString("D6" + ".jpg"));

            }


        }


        public Image DownloadImage(string _URL)
        {
            Image _tmpImage = null;

            try
            {
                // Open a connection
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_URL);

                _HttpWebRequest.AllowWriteStreamBuffering = true;

                // You can also specify additional header values like the user agent or the referer: (Optional)
                //_HttpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
                //_HttpWebRequest.Referer = "http://www.google.com/";

                // set timeout for 20 seconds (Optional)
                _HttpWebRequest.Timeout = 60000;

                // Request response:
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();

                // Open data stream:
                System.IO.Stream _WebStream = _WebResponse.GetResponseStream();

                // convert webstream to image
                _tmpImage = Image.FromStream(_WebStream);

                // Cleanup
                _WebResponse.Close();
                _WebResponse.Close();
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
                return null;
            }

            return _tmpImage;
        }