// Copyright (c) 2015 Ravi Bhavnani
// License: Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Collections;

namespace biodanza
{
    /// <summary>
    /// Translates text using Google's online language tools.
    /// </summary>
    public class Translator
    {
        #region Properties


            /// <summary>
            /// Gets the time taken to perform the translation.
            /// </summary>
            public TimeSpan TranslationTime {
                get;
                private set;
            }

            /// <summary>
            /// Gets the url used to speak the translation.
            /// </summary>
            /// <value>The url used to speak the translation.</value>
            public string TranslationSpeechUrl {
                get;
                private set;
            }

            /// <summary>
            /// Gets the error.
            /// </summary>
            public Exception Error {
                get;
                private set;
            }

        #endregion

        #region Public methods

            /// <summary>
            /// Translates the specified source text.
            /// </summary>
            /// <param name="sourceText">The source text.</param>
            /// <param name="sourceLanguage">The source language.</param>
            /// <param name="targetLanguage">The target language.</param>
            /// <returns>The translation.</returns>
            public string Translate
                (string sourceText,
                 string sourceLanguage,
                 string targetLanguage,
                 bool decode = true)
            {
                // Initialize
                this.Error = null;
                this.TranslationSpeechUrl = null;
                this.TranslationTime = TimeSpan.Zero;
                DateTime tmStart = DateTime.Now;
                string translation = string.Empty;

                try {
                    // Download translation
                    sourceText = sourceText.Replace('\r', '~');
                    sourceText = sourceText.Replace('\n', ' ');
                    sourceText = sourceText.Replace('\"', '\'');
                    sourceText = sourceText.Replace('\\', ' ');
                    

                    ASCIIEncoding ascii = new ASCIIEncoding();
                    byte[] byteArray = Encoding.UTF8.GetBytes(sourceText);
                    byte[] asciiArray = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, byteArray);
                    string finalString = ascii.GetString(asciiArray);
                    String sf = finalString;
                    sf = sourceText;
                   
                    string url = string.Format ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                                                sourceLanguage, 
                                                Translator.LanguageEnumToIdentifier (targetLanguage),
                                                HttpUtility.UrlEncode (sf));

                    string outputFile = Path.GetTempFileName();
                    using (WebClient wc = new WebClient ()) {
                        wc.Headers.Add ("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
                        wc.DownloadFile(url, outputFile);
                    }

                    // Get translated text
                    if ( File.Exists (outputFile) ) {
                    // Get phrase collection
                        string text = File.ReadAllText(outputFile);
                    if (decode){
                        text = DecodeMessageTrans(text);
                    }

                        return text;
                    }
                }
                catch (Exception ex) {
                    this.Error = ex;
                }

                // Return result
                this.TranslationTime = DateTime.Now - tmStart;
                return translation;
            }

        #endregion

        #region Private methods

            /// <summary>
            /// Converts a language to its identifier.
            /// </summary>
            /// <param name="language">The language."</param>
            /// <returns>The identifier or <see cref="string.Empty"/> if none.</returns>
            private static string LanguageEnumToIdentifier
                (string language)
            {
                string mode = string.Empty;
                Translator.EnsureInitialized();
                mode = Translator._languageModeMap[language];
                return mode;
            }

            /// <summary>
            /// Ensures the translator has been initialized.
            /// </summary>
            private static void EnsureInitialized()
            {
                if (Translator._languageMapMode == null) {
                    Translator._languageMapMode = new Dictionary<string,string>();
                    Translator._languageMapMode.Add ( "af","Afrikaans"        );
                    Translator._languageMapMode.Add ( "sq","Albanian"         );
                    Translator._languageMapMode.Add ( "ar","Arabic"           );
                    Translator._languageMapMode.Add ( "hy","Armenian"         );
                    Translator._languageMapMode.Add ( "az","Azerbaijani"      );
                    Translator._languageMapMode.Add ( "eu","Basque"           );
                    Translator._languageMapMode.Add ( "be","Belarusian"       );
                    Translator._languageMapMode.Add ( "bn","Bengali"          );
                    Translator._languageMapMode.Add ( "bg","Bulgarian"        );
                    Translator._languageMapMode.Add ( "ca","Catalan"          );
                    Translator._languageMapMode.Add ("zh-CN","Chinese"       );
                    Translator._languageMapMode.Add ( "hr","Croatian"         );
                    Translator._languageMapMode.Add ( "cs","Czech"            );
                    Translator._languageMapMode.Add ( "da","Danish"           );
                    Translator._languageMapMode.Add ( "nl","Dutch"            );
                    Translator._languageMapMode.Add ( "en","English"          );
                    Translator._languageMapMode.Add ( "eo","Esperanto"        );
                    Translator._languageMapMode.Add ( "et","Estonian"         );
                    Translator._languageMapMode.Add ( "tl","Filipino"         );
                    Translator._languageMapMode.Add ( "fi","Finnish"          );
                    Translator._languageMapMode.Add ( "fr","French"           );
                    Translator._languageMapMode.Add ( "gl","Galician"         );
                    Translator._languageMapMode.Add ( "de","German"           );
                    Translator._languageMapMode.Add ( "ka","Georgian"         );
                    Translator._languageMapMode.Add ( "el","Greek"            );
                    Translator._languageMapMode.Add ( "ht","Haitian Creole"   );
                    Translator._languageMapMode.Add ( "iw","Hebrew"           );
                    Translator._languageMapMode.Add ( "hi","Hindi"            );
                    Translator._languageMapMode.Add ( "hu","Hungarian"        );
                    Translator._languageMapMode.Add ( "is","Icelandic"        );
                    Translator._languageMapMode.Add ( "id","Indonesian"       );
                    Translator._languageMapMode.Add ( "ga","Irish"            );
                    Translator._languageMapMode.Add ( "it","Italian"          );
                    Translator._languageMapMode.Add ( "ja","Japanese"         );
                    Translator._languageMapMode.Add ( "ko","Korean"           );
                    Translator._languageMapMode.Add ( "lo","Lao"              );
                    Translator._languageMapMode.Add ( "la","Latin"            );
                    Translator._languageMapMode.Add ( "lv","Latvian"          );
                    Translator._languageMapMode.Add ( "lt","Lithuanian"       );
                    Translator._languageMapMode.Add ( "mk","Macedonian"       );
                    Translator._languageMapMode.Add ( "ms","Malay"            );
                    Translator._languageMapMode.Add ( "mt","Maltese"          );
                    Translator._languageMapMode.Add ( "no","Norwegian"        );
                    Translator._languageMapMode.Add ( "fa","Persian"          );
                    Translator._languageMapMode.Add ( "pl","Polish"           );
                    Translator._languageMapMode.Add ( "pt","Portuguese"       );
                    Translator._languageMapMode.Add ( "ro","Romanian"         );
                    Translator._languageMapMode.Add ( "ru","Russian"          );
                    Translator._languageMapMode.Add ( "sr","Serbian"          );
                    Translator._languageMapMode.Add ( "sk","Slovak"           );
                    Translator._languageMapMode.Add ( "sl","Slovenian"        );
                    Translator._languageMapMode.Add ( "es","Spanish"          );
                    Translator._languageMapMode.Add ( "sw","Swahili"          );
                    Translator._languageMapMode.Add ( "sv","Swedish"          );
                    Translator._languageMapMode.Add ( "ta","Tamil"            );
                    Translator._languageMapMode.Add ( "te","Telugu"           );
                    Translator._languageMapMode.Add ( "th","Thai"             );
                    Translator._languageMapMode.Add ( "tr","Turkish"          );
                    Translator._languageMapMode.Add ( "uk","Ukrainian"        );
                    Translator._languageMapMode.Add ( "ur","Urdu"             );
                    Translator._languageMapMode.Add ( "vi","Vietnamese"       );
                    Translator._languageMapMode.Add ( "cy","Welsh"            );
                    Translator._languageMapMode.Add(  "yi","Yiddish"          );
                }
            if (Translator._languageModeMap == null)
            {
                Translator._languageModeMap = new Dictionary<string, string>();
                Translator._languageModeMap.Add("Afrikaans", "af");
                Translator._languageModeMap.Add("Albanian", "sq");
                Translator._languageModeMap.Add("Arabic", "ar");
                Translator._languageModeMap.Add("Armenian", "hy");
                Translator._languageModeMap.Add("Azerbaijani", "az");
                Translator._languageModeMap.Add("Basque", "eu");
                Translator._languageModeMap.Add("Belarusian", "be");
                Translator._languageModeMap.Add("Bengali", "bn");
                Translator._languageModeMap.Add("Bulgarian", "bg");
                Translator._languageModeMap.Add("Catalan", "ca");
                Translator._languageModeMap.Add("Chinese", "zh-CN");
                Translator._languageModeMap.Add("Croatian", "hr");
                Translator._languageModeMap.Add("Czech", "cs");
                Translator._languageModeMap.Add("Danish", "da");
                Translator._languageModeMap.Add("Dutch", "nl");
                Translator._languageModeMap.Add("English", "en");
                Translator._languageModeMap.Add("Esperanto", "eo");
                Translator._languageModeMap.Add("Estonian", "et");
                Translator._languageModeMap.Add("Filipino", "tl");
                Translator._languageModeMap.Add("Finnish", "fi");
                Translator._languageModeMap.Add("French", "fr");
                Translator._languageModeMap.Add("Galician", "gl");
                Translator._languageModeMap.Add("German", "de");
                Translator._languageModeMap.Add("Georgian", "ka");
                Translator._languageModeMap.Add("Greek", "el");
                Translator._languageModeMap.Add("Haitian Creole", "ht");
                Translator._languageModeMap.Add("Hebrew", "iw");
                Translator._languageModeMap.Add("Hindi", "hi");
                Translator._languageModeMap.Add("Hungarian", "hu");
                Translator._languageModeMap.Add("Icelandic", "is");
                Translator._languageModeMap.Add("Indonesian", "id");
                Translator._languageModeMap.Add("Irish", "ga");
                Translator._languageModeMap.Add("Italian", "it");
                Translator._languageModeMap.Add("Japanese", "ja");
                Translator._languageModeMap.Add("Korean", "ko");
                Translator._languageModeMap.Add("Lao", "lo");
                Translator._languageModeMap.Add("Latin", "la");
                Translator._languageModeMap.Add("Latvian", "lv");
                Translator._languageModeMap.Add("Lithuanian", "lt");
                Translator._languageModeMap.Add("Macedonian", "mk");
                Translator._languageModeMap.Add("Malay", "ms");
                Translator._languageModeMap.Add("Maltese", "mt");
                Translator._languageModeMap.Add("Norwegian", "no");
                Translator._languageModeMap.Add("Persian", "fa");
                Translator._languageModeMap.Add("Polish", "pl");
                Translator._languageModeMap.Add("Portuguese", "pt");
                Translator._languageModeMap.Add("Romanian", "ro");
                Translator._languageModeMap.Add("Russian", "ru");
                Translator._languageModeMap.Add("Serbian", "sr");
                Translator._languageModeMap.Add("Slovak", "sk");
                Translator._languageModeMap.Add("Slovenian", "sl");
                Translator._languageModeMap.Add("Spanish", "es");
                Translator._languageModeMap.Add("Swahili", "sw");
                Translator._languageModeMap.Add("Swedish", "sv");
                Translator._languageModeMap.Add("Tamil", "ta");
                Translator._languageModeMap.Add("Telugu", "te");
                Translator._languageModeMap.Add("Thai", "th");
                Translator._languageModeMap.Add("Turkish", "tr");
                Translator._languageModeMap.Add("Ukrainian", "uk");
                Translator._languageModeMap.Add("Urdu", "ur");
                Translator._languageModeMap.Add("Vietnamese", "vi");
                Translator._languageModeMap.Add("Welsh", "cy");
                Translator._languageModeMap.Add("Yiddish", "yi");
            }
        }

        private static string DecodeMessageTrans2(string message)
        {
            int en = 0;
            string token1 = "[";
            string token2 = "\",";
            string result = "";
            string linea = "";

            message = message.Substring(2,message.IndexOf("]],")-3);
            while ( ( en = message.IndexOf(token1, en) ) != -1 )
            {
                int nEn2 = message.IndexOf(token2, en);
                if (nEn2 > 0)
                {
                    Int32 len = nEn2 - en - 2;
                    if (len <= 0)
                        break;
                    linea = message.Substring(en + 2, len);
                    result += linea + " ";
                }
                en++;
            }
            result = result.Replace('\"', '\'');
            result = result.Replace('\\', ' ');


            return result;

        }
        private static string DecodeMessageTrans(string message)
        {
            string token1 = "[";
            string result = "";
            
            message = message.Substring(3, message.Length - 4);
            
            message = message.Replace(token1, "^");
            string[] lineas = message.Split('^');

            foreach (string linea in lineas)
            {
                if (string.IsNullOrEmpty(linea))
                    break;
                string linea2 = linea.Substring(1);
                result += linea2.Substring(0, linea2.IndexOf('"'));
            }

            result = result.Replace('\"', '\'');
            result = result.Replace('\\', ' ');


            return result;

        }

        public string Language(string text)
        {
            string lan = string.Empty;
            string textTra = Translate(text, "auto", "Spanish", false);
            string ret = "";

            string prefix = textTra.Substring(textTra.Length - 6, 2); 
            if(_languageMapMode.ContainsKey(prefix))
            {
                ret = _languageMapMode[prefix];
            }
            return ret;
        }

        #endregion

        #region Fields

            /// <summary>
            /// The language to translation mode map.
            /// </summary>
            private static Dictionary<string, string> _languageModeMap;
            private static Dictionary<string, string> _languageMapMode;

        #endregion
    }
}
