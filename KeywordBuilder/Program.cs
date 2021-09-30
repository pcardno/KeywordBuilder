using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KeywordBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //List<Asset> assets = JsonConvert.DeserializeObject<List<Asset>>(File.ReadAllText(@"C:\wipro\keywords\asset_tags_docs_20210802.json"));

            List<string> inputFilesList = new();

            inputFilesList.Add(@"C:\wipro\keywords\document_asset_output.json");
            inputFilesList.Add(@"C:\wipro\keywords\image_asset_output.json");
            inputFilesList.Add(@"C:\wipro\keywords\video_asset_output_part_1.json");
            inputFilesList.Add(@"C:\wipro\keywords\video_asset_output_part_2.json");
            inputFilesList.Add(@"C:\wipro\keywords\video_asset_output_part_3.json");
            inputFilesList.Add(@"C:\wipro\keywords\video_asset_output_part_4.json");
            inputFilesList.Add(@"C:\wipro\keywords\video_asset_output_part_5.json");
            inputFilesList.Add(@"C:\wipro\keywords\video_asset_output_part_6.json");
            inputFilesList.Add(@"C:\wipro\keywords\zip_asset_output.json");


            List<Asset> assetsFromWipro = new();

            foreach(string inputFile in inputFilesList)
            {
                List<Asset> interimList = JsonConvert.DeserializeObject<List<Asset>>(File.ReadAllText(inputFile));

                assetsFromWipro.AddRange(interimList);

            }

            string[] assetsWithNoCelumKeywords = File.ReadAllLines(@"C:\wipro\keywords\assetswithnokeywords.txt");

            Dictionary<string, int> allKeywords = new();

            foreach(Asset asset in assetsFromWipro)
            {
                if(asset.Keywords != null)
                { 
                    foreach(string keyword in asset.Keywords)
                    {
                        if(allKeywords.ContainsKey(keyword))
                        {
                            allKeywords[keyword]++;
                        }
                        else
                        {
                            allKeywords.Add(keyword, 1);
                        }
                    }
                }
                if (asset.Components != null)
                {
                    foreach(Asset component in asset.Components)
                    {
                        if (component.Keywords != null)
                        {
                            foreach (string keyword in component.Keywords)
                            {
                                if (allKeywords.ContainsKey(keyword))
                                {
                                    allKeywords[keyword]++;
                                }
                                else
                                {
                                    allKeywords.Add(keyword, 1);
                                }
                            }
                        }
                    }
                }
            }

            List<string> outputAllKeywords = new();
            foreach(string keyword in allKeywords.Keys)
            {
                string outputAllKeywordsString = String.Format("{0}\t{1}", keyword, allKeywords[keyword]);

                outputAllKeywords.Add(outputAllKeywordsString);
            }

            File.WriteAllLines(@"C:\wipro\keywords\pivotonkeywords.txt", outputAllKeywords);

            Dictionary<string, Asset> assetDictionary = new();


            foreach (Asset asset in assetsFromWipro)
            {
                if (!assetDictionary.ContainsKey(asset.AssetID))
                {

                    assetDictionary.Add(asset.AssetID, asset); 
                }
            }

            List<string> outputString = new();

            foreach(string assetID in assetsWithNoCelumKeywords)
            {
                if ((assetDictionary.ContainsKey(assetID)))
                {
                    Asset asset = assetDictionary[assetID];

                    //Check if it was a ZIP file
                    if ((asset.Components != null)&&(asset.Components.Count > 0))
                    {
                        Dictionary<string, bool> uniqueKeywords = new();

                        List<string> uniqueKeywordsList = new();

                        //it was a zip file
                        foreach(Asset component in asset.Components)
                        {
                            if (component.Keywords != null)
                            {
                                foreach (string keyword in component.Keywords)
                                {
                                    if (!uniqueKeywords.ContainsKey(keyword))
                                    {
                                        uniqueKeywords.Add(keyword, true);

                                        uniqueKeywordsList.Add(keyword);
                                    }
                                }

                            }
                        }

                        string keywords = String.Join(", ", uniqueKeywordsList);

                        outputString.Add(String.Format("{0}\t{1}\tZipFile", assetID, keywords));

                    }
                    else //it wasn't a zip file
                    {
                        if (asset.Keywords != null)
                        {
                            asset.UpdatedKeywords = true;

                            string keywords = String.Join(", ", asset.Keywords);

                            outputString.Add(String.Format("{0}\t{1}\tImageorDoc", assetID, keywords));
                        }
                    }
                }
                else
                {

                }
            }

            File.WriteAllLines(@"c:\wipro\keywords\keywordstoload.txt", outputString);            
            
            Console.WriteLine("end");
             
        }

    }
}
