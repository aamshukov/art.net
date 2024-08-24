//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using Art.Framework.Json;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace UILab.Art.Tests;

[TestFixture]
internal class JsonTests
{
    private static void Validate(JsonPatchBuilder builder, string lhsJson, string? resultJson = default)
    {
        string script = builder.GenerateScript();
        Assert.That(script, Is.Not.EqualTo(JsonPatchBuilder.EmptyDocument));
        var lhsObject = JsonConvert.DeserializeObject(lhsJson);
        builder.Document.ApplyTo(lhsObject);
        string lhsPatchedJson = JsonConvert.SerializeObject(lhsObject);
        //Console.WriteLine(lhsPatchedJson);
        if(resultJson is not null)
        {
            var lhsPatchedObject = JsonConvert.DeserializeObject(lhsPatchedJson);
            var resultObject = JsonConvert.DeserializeObject(resultJson);
            Assert.That(lhsPatchedObject, Is.EqualTo(resultObject));
        }
    }

    [Test]
    public void JsonPatchBuilder_Build_Object_Success()
    {
        JsonPatchDocument document = new();
        JsonPatchBuilder builder = new(document);

        string lhsJson =
        @"
        {
            ""lhs-bar"": ""bar"",
            ""lhs-foo"": ""foo"",
            ""lhs-zzz"": ""zzz""
        }
        ";

        string rhsJson =
        @"
        {
            ""rhs-baz"": ""baz"",
            ""rhs-foo"": ""123"",
            ""lhs-zzz"": ""xyz""
        }
        ";

        string resultJson = @"{""lhs-zzz"":""xyz"",""rhs-baz"":""baz"",""rhs-foo"":""123""}";

        builder.Build(lhsJson, rhsJson);
        Validate(builder, lhsJson, resultJson);
    }

    [Test]
    public void JsonPatchBuilder_Build_NestedObject_Success()
    {
        JsonPatchDocument document = new();
        JsonPatchBuilder builder = new(document);

        string lhsJson =
        @"
        {
            ""employee"":
            {
                ""lhs-name"": ""John"",
                ""lhs-middlename"": null,
                ""lhs-age"": 30,
                ""lhs-bool"": true,
                ""lhs-double"": 3.14156,
                ""lhs-decimal"": 100.198375893475893490485609458694504156,
                ""lhs-city"": ""New York""
            },
            ""lhs-baz"": ""baz"",
            ""lhs-foo"": ""12345""
        }
        ";

        string rhsJson =
        @"
        {
            ""employee"":
            {
                ""rhs-name"": ""Art"",
                ""rhs-middlename"": ""MiddleName"",
                ""rhs-age"": 50,
                ""rhs-bool"": true,
                ""rhs-double"": 0.7378,
                ""rhs-decimal"": 100.198375893475893490485609458694504156,
                ""rhs-city"": ""New York""
            },
            ""rhs-baz"": ""bar"",
            ""rhs-foo"": ""12345""
        }
        ";

        string resultJson = @"{""employee"":{""rhs-name"":""Art"",""rhs-middlename"":""MiddleName"",""rhs-age"":50,""rhs-bool"":true,""rhs-double"":0.7378,""rhs-decimal"":100.1983758934759,""rhs-city"":""New York""},""rhs-baz"":""bar"",""rhs-foo"":""12345""}";

        builder.Build(lhsJson, rhsJson);
        Validate(builder, lhsJson, resultJson);
    }

    [Test]
    public void JsonPatchBuilder_Build_Objects_Success()
    {
        JsonPatchDocument document = new();
        JsonPatchBuilder builder = new(document);

        string lhsJson =
        @"
        {
          ""example"": 1534290314.9101248,
          ""stopped"": {
            ""art"": {
              ""needle"": {
                ""flower"": {
                  ""freedom"": {
                    ""nobody"": {
                      ""rough"": true,
                      ""check"": ""president"",
                      ""neighbor"": ""anywhere"",
                      ""dug"": {
                        ""push"": false,
                        ""review"": -683005372.3265395,
                        ""its"": {
                          ""pink"": false,
                          ""circus"": false,
                          ""temperature"": ""refer"",
                          ""wide"": {
                            ""reach"": ""sheep"",
                            ""nervous"": ""important"",
                            ""acres"": ""doll"",
                            ""sat"": {
                              ""box"": ""baseball"",
                              ""union"": {
                                ""air"": ""horn"",
                                ""speed"": {
                                  ""equal"": {
                                    ""sentence"": false,
                                    ""lamp"": {
                                      ""accept"": -774976404.3664947,
                                      ""immediately"": true,
                                      ""paid"": -1197523804.1281624,
                                      ""thrown"": 460443433.5610261,
                                      ""instance"": false,
                                      ""eleven"": true,
                                      ""log"": {
                                        ""century"": {
                                          ""square"": true,
                                          ""said"": {
                                            ""breeze"": 1916357470,
                                            ""warn"": ""sets"",
                                            ""divide"": {
                                              ""outer"": {
                                                ""energy"": true,
                                                ""jar"": 1023248103,
                                                ""bank"": false,
                                                ""mark"": -1905665185,
                                                ""tales"": 1430626651,
                                                ""plates"": -756893250,
                                                ""exist"": 1407385395,
                                                ""recently"": false,
                                                ""friend"": ""wool"",
                                                ""means"": true,
                                                ""station"": 654480149.140624,
                                                ""being"": -1274769943.899497,
                                                ""dried"": true,
                                                ""snake"": ""gentle"",
                                                ""might"": ""hit"",
                                                ""glad"": 1381201430,
                                                ""till"": -1670010495.2616973,
                                                ""kill"": -366939805.2548132,
                                                ""few"": false,
                                                ""build"": -544635158
                                              },
                                              ""fifty"": false,
                                              ""bent"": false,
                                              ""nails"": -1696854432,
                                              ""captured"": false,
                                              ""listen"": false,
                                              ""sing"": ""ring"",
                                              ""lost"": false,
                                              ""roof"": 465417804.79114723,
                                              ""force"": 222532515,
                                              ""meant"": false,
                                              ""pool"": false,
                                              ""salmon"": ""struggle"",
                                              ""grass"": true,
                                              ""shoot"": false,
                                              ""famous"": -96309332,
                                              ""calm"": ""hurried"",
                                              ""avoid"": true,
                                              ""flow"": ""include"",
                                              ""supply"": false
                                            },
                                            ""ground"": 716290261,
                                            ""strip"": false,
                                            ""planet"": false,
                                            ""likely"": -1728175808,
                                            ""grain"": true,
                                            ""happen"": false,
                                            ""prevent"": 678444969,
                                            ""this"": ""visit"",
                                            ""feathers"": true,
                                            ""thus"": ""asleep"",
                                            ""food"": ""oxygen"",
                                            ""travel"": true,
                                            ""real"": ""discover"",
                                            ""was"": -985517112,
                                            ""exchange"": false,
                                            ""fun"": ""snow"",
                                            ""found"": false
                                          },
                                          ""stuck"": -878140250,
                                          ""judge"": true,
                                          ""plain"": 783772141,
                                          ""both"": false,
                                          ""quietly"": ""girl"",
                                          ""vegetable"": ""television"",
                                          ""additional"": ""notice"",
                                          ""largest"": true,
                                          ""forgot"": ""express"",
                                          ""strange"": ""pilot"",
                                          ""nails"": true,
                                          ""won"": true,
                                          ""model"": ""shut"",
                                          ""library"": -277408203.74498034,
                                          ""express"": ""adjective"",
                                          ""win"": -849395751,
                                          ""part"": false,
                                          ""floor"": 531722525.9390764
                                        },
                                        ""ice"": -1176498784.0766954,
                                        ""palace"": false,
                                        ""written"": false,
                                        ""tree"": 788506274,
                                        ""skin"": ""bear"",
                                        ""pride"": -30791077.135849,
                                        ""party"": 907379963,
                                        ""strike"": -431571838,
                                        ""influence"": ""slowly"",
                                        ""skill"": 622673216.5241747,
                                        ""fewer"": true,
                                        ""at"": 747764564.8671827,
                                        ""meat"": true,
                                        ""guess"": 393067629.89818907,
                                        ""ready"": -139356718,
                                        ""jump"": ""policeman"",
                                        ""circus"": -62192119.051012516,
                                        ""evening"": ""unusual"",
                                        ""could"": ""wall""
                                      },
                                      ""told"": 1462583089,
                                      ""aid"": ""modern"",
                                      ""mind"": ""giving"",
                                      ""dark"": ""massage"",
                                      ""weigh"": ""though"",
                                      ""open"": ""hollow"",
                                      ""forgot"": ""sick"",
                                      ""duty"": -74606802.38269663,
                                      ""finest"": -1722517566.6456804,
                                      ""road"": ""difficulty"",
                                      ""pocket"": true,
                                      ""scared"": -1620215631.500947,
                                      ""fewer"": true
                                    },
                                    ""yourself"": -2035838206,
                                    ""pair"": ""find"",
                                    ""all"": 962029138.622015,
                                    ""up"": ""opposite"",
                                    ""physical"": ""saved"",
                                    ""trouble"": 62753573.47688389,
                                    ""seed"": -1092787635,
                                    ""again"": -1327760957.329083,
                                    ""already"": 1372405852,
                                    ""wonderful"": true,
                                    ""control"": 1868623667,
                                    ""weight"": true,
                                    ""luck"": true,
                                    ""with"": -1374445775,
                                    ""trade"": 1771235530.328785,
                                    ""harbor"": ""wood"",
                                    ""tin"": 2093008239,
                                    ""post"": 1796796858.7286248
                                  },
                                  ""slept"": ""occur"",
                                  ""attached"": true,
                                  ""married"": true,
                                  ""frog"": 1647978505,
                                  ""bag"": ""practical"",
                                  ""soap"": 326771761.07871723,
                                  ""reason"": -859087939,
                                  ""rule"": 314596237,
                                  ""near"": ""recall"",
                                  ""species"": ""ground"",
                                  ""pound"": true,
                                  ""moving"": -1508143383.4633331,
                                  ""create"": ""improve"",
                                  ""shoe"": -1870495906.9353666,
                                  ""thumb"": false,
                                  ""whatever"": true,
                                  ""before"": ""strange"",
                                  ""stranger"": -718733287.8546143,
                                  ""cell"": 738706445.0638833
                                },
                                ""limited"": true,
                                ""chest"": ""goose"",
                                ""enemy"": ""wave"",
                                ""told"": ""deep"",
                                ""rhyme"": ""near"",
                                ""men"": false,
                                ""hour"": 1776269235.4351683,
                                ""even"": false,
                                ""negative"": ""exclaimed"",
                                ""cage"": ""remove"",
                                ""aboard"": ""shade"",
                                ""life"": 1393316984,
                                ""thick"": -82495657,
                                ""mostly"": true,
                                ""hard"": false,
                                ""different"": false,
                                ""putting"": ""sail"",
                                ""character"": true
                              },
                              ""supper"": ""century"",
                              ""provide"": -1712814197.4500709,
                              ""properly"": 750861335.8222594,
                              ""round"": 277570787,
                              ""shot"": ""themselves"",
                              ""regular"": ""stone"",
                              ""bee"": 829180643,
                              ""ship"": 1710810028.4650874,
                              ""pitch"": 1029413142.5161576,
                              ""spend"": -365146499.2960572,
                              ""sang"": ""view"",
                              ""plural"": 1315344433,
                              ""look"": ""rich"",
                              ""moon"": ""having"",
                              ""pony"": 712848900.1315684,
                              ""map"": false,
                              ""ourselves"": ""amount"",
                              ""come"": false
                            },
                            ""drew"": true,
                            ""ability"": 855608272,
                            ""pain"": -1579567380.59895,
                            ""ask"": false,
                            ""aboard"": 978310359,
                            ""easily"": true,
                            ""poem"": 2108557131,
                            ""sang"": ""supper"",
                            ""condition"": true,
                            ""stuck"": ""price"",
                            ""using"": 225975373.63759518,
                            ""mainly"": true,
                            ""automobile"": ""curious"",
                            ""arrange"": ""sing"",
                            ""rays"": ""local"",
                            ""he"": -472279284.95319796
                          },
                          ""coast"": ""floating"",
                          ""also"": false,
                          ""standard"": ""eager"",
                          ""parts"": false,
                          ""highest"": true,
                          ""machinery"": -81784435,
                          ""figure"": true,
                          ""longer"": false,
                          ""blanket"": false,
                          ""observe"": ""owner"",
                          ""through"": 93504388,
                          ""transportation"": -1944101030.3073735,
                          ""see"": 602523174.2340493,
                          ""impossible"": true,
                          ""unless"": ""winter"",
                          ""potatoes"": -1606427739.8719635
                        },
                        ""winter"": -1251616633.2613258,
                        ""statement"": -168825593.64195156,
                        ""table"": -90045773,
                        ""needs"": -1569675836.8261213,
                        ""nor"": 727748607,
                        ""plates"": true,
                        ""directly"": ""exist"",
                        ""dog"": false,
                        ""folks"": false,
                        ""afraid"": ""probably"",
                        ""include"": 54508662,
                        ""village"": true,
                        ""aside"": ""knowledge"",
                        ""object"": true,
                        ""respect"": -959017841,
                        ""produce"": -495807393.21898746
                      },
                      ""build"": true,
                      ""climb"": ""lake"",
                      ""sitting"": -1744699828,
                      ""average"": ""tonight"",
                      ""on"": false,
                      ""nose"": false,
                      ""gray"": ""complex"",
                      ""scale"": ""yet"",
                      ""driving"": ""meal"",
                      ""regular"": true,
                      ""meet"": 1182364128.9669104,
                      ""live"": false,
                      ""start"": false,
                      ""hole"": -2142645537.7452168,
                      ""please"": ""across"",
                      ""serve"": ""bus""
                    },
                    ""buy"": -59203623,
                    ""applied"": ""order"",
                    ""somehow"": ""allow"",
                    ""military"": false,
                    ""garden"": ""upward"",
                    ""die"": true,
                    ""similar"": ""trace"",
                    ""play"": ""ranch"",
                    ""rubber"": true,
                    ""golden"": ""can"",
                    ""pack"": 2008177628.2885685,
                    ""fifteen"": 299163808,
                    ""wing"": 2137406846,
                    ""comfortable"": 1764524087,
                    ""concerned"": false,
                    ""occasionally"": ""plate"",
                    ""shore"": -1356140146.895183,
                    ""husband"": ""swept"",
                    ""everything"": 1172317544
                  },
                  ""cutting"": true,
                  ""get"": ""stepped"",
                  ""ordinary"": -471878501,
                  ""shelter"": false,
                  ""seven"": 1162882314,
                  ""frozen"": 391927097.714201,
                  ""mathematics"": ""living"",
                  ""brick"": ""single"",
                  ""heading"": true,
                  ""control"": ""fed"",
                  ""whistle"": ""substance"",
                  ""cover"": ""type"",
                  ""official"": ""indicate"",
                  ""teacher"": false,
                  ""perfect"": true,
                  ""acres"": ""fox"",
                  ""seldom"": false,
                  ""will"": ""start"",
                  ""gray"": ""soap""
                },
                ""stared"": ""captured"",
                ""poem"": -151486890.62004042,
                ""gone"": ""fifth"",
                ""notice"": ""where"",
                ""about"": true,
                ""object"": ""park"",
                ""bush"": ""wet"",
                ""dead"": false,
                ""appropriate"": ""image"",
                ""whispered"": false,
                ""tax"": ""badly"",
                ""found"": ""pink"",
                ""design"": true,
                ""constantly"": ""industrial"",
                ""ought"": -2029710866.0868988,
                ""lady"": ""establish"",
                ""choice"": -1068665655,
                ""tales"": true,
                ""elephant"": ""understanding""
              },
              ""average"": ""plenty"",
              ""owner"": ""handle"",
              ""box"": -36740844.12833452,
              ""crowd"": ""moon"",
              ""basis"": 107069651,
              ""tip"": ""asleep"",
              ""remarkable"": -1781012228.7343311,
              ""herself"": true,
              ""previous"": ""treated"",
              ""field"": ""taste"",
              ""arm"": false,
              ""signal"": 1586455580,
              ""clothing"": true,
              ""several"": ""sign"",
              ""wealth"": false,
              ""special"": false,
              ""dark"": 685272920.9289565,
              ""ordinary"": true,
              ""tank"": ""local""
            },
            ""trouble"": ""feel"",
            ""pain"": false,
            ""everyone"": true,
            ""tried"": false,
            ""rose"": ""article"",
            ""variety"": 1732545636,
            ""maybe"": -750404433,
            ""origin"": ""body"",
            ""like"": 90123173,
            ""baby"": ""asleep"",
            ""gulf"": 1883126352,
            ""captured"": false,
            ""as"": 431506474.5715847,
            ""person"": ""die"",
            ""remember"": -353608523,
            ""shall"": -1196894557.9894834,
            ""signal"": true,
            ""spring"": false,
            ""grandfather"": 747056846
          },
          ""even"": ""mostly"",
          ""happily"": true,
          ""show"": 1666045037,
          ""learn"": -919494756,
          ""wear"": ""rise"",
          ""shoe"": true,
          ""sat"": false,
          ""fewer"": -1329101547,
          ""train"": -1797244273.243369,
          ""ordinary"": 1933136390,
          ""refer"": -1889578735,
          ""chief"": true,
          ""explanation"": ""attack"",
          ""line"": ""program"",
          ""soon"": true,
          ""upper"": ""frequently"",
          ""spend"": true,
          ""system"": -1092013327.7400465
        }
        ";

        string rhsJson =
        @"
        {
          ""even"": ""MOSTLY"",
          ""happily"": true,
          ""show"": 51666045037,
          ""learn"": 919494756,
          ""wear"": ""RISE"",
          ""shoe"": false,
          ""sat"": false,
          ""fewer"": 1329101547,
          ""train"": 1797244273.243369,
          ""ordinary"": 1933136390,
          ""refer"": 1889578735,
          ""chief"": true,
          ""explanation"": ""ATTACK"",
          ""line"": ""PROGRAM"",
          ""soon"": true,
          ""upper"": ""FREQUENTLY"",
          ""spend"": true,
          ""system"": 1092013327.7400465
        }
        ";

        string resultJson = @"{""even"":""MOSTLY"",""happily"":true,""show"":51666045037,""learn"":919494756,""wear"":""RISE"",""shoe"":false,""sat"":false,""fewer"":1329101547,""train"":1797244273.243369,""ordinary"":1933136390,""refer"":1889578735,""chief"":true,""explanation"":""ATTACK"",""line"":""PROGRAM"",""soon"":true,""upper"":""FREQUENTLY"",""spend"":true,""system"":1092013327.7400465}";

        builder.Build(lhsJson, rhsJson);
        Validate(builder, lhsJson, resultJson);
    }

    [Test]
    public void JsonPatchBuilder_Build_Array_Success()
    {
        JsonPatchDocument document = new();
        JsonPatchBuilder builder = new(document);

        string lhsJson =
        @"
        {
            ""employees"":[""John"", ""Zakh"", ""Anna""]
        }
        ";

        string rhsJson =
        @"
        {
            ""employees"":[""Art"", ""Zakh"", ""Van""]
        }
        ";

        string resultJson = @"{""employees"":[""Art"",""Zakh"",""Van""]}";

        builder.Build(lhsJson, rhsJson);
        Validate(builder, lhsJson, resultJson);
    }

    [Test]
    public void JsonGenerator_Generate_Random_Success()
    {
        int count = 10;

        for(int k = 0; k < count; k++)
        {
            JsonGenerator jsonGenerator = new(maxStringLength: 16,
                                              maxArrayLength: 12,
                                              maxObjectProperties: 16,
                                              maxObjectPropertyLength: 12,
                                              maxDepth: 10);
            string jsonText = jsonGenerator.GenerateRandomJson(k % 2 == 0 ? JsonGenerator.DataType.Object : JsonGenerator.DataType.Array);
            //Console.WriteLine(jsonText);
            Assert.That(jsonText, Is.Not.Empty);
        }
    }

    [Test]
    public void JsonPatchBuilder_Generate_Build_Random_Success()
    {
        int count = 10;

        JsonPatchDocument document = new();
        JsonPatchBuilder builder = new(document);

        for(int k = 0; k < count; k++)
        {
            JsonGenerator jsonGenerator = new(maxStringLength: 8,
                                              maxArrayLength: 12,
                                              maxObjectProperties: 8,
                                              maxObjectPropertyLength: 12,
                                              maxDepth: 10);
            string lhsJson = jsonGenerator.GenerateRandomJson(k % 2 == 0 ? JsonGenerator.DataType.Object : JsonGenerator.DataType.Array);
            string rhsJson = jsonGenerator.GenerateRandomJson(k % 2 == 0 ? JsonGenerator.DataType.Object : JsonGenerator.DataType.Array);

            //Console.WriteLine("LHS:");
            //Console.WriteLine(lhsJson);
            //Console.WriteLine();
            //Console.WriteLine("RHS:");
            //Console.WriteLine(rhsJson);

            builder.Build(lhsJson, rhsJson);
            Validate(builder, lhsJson);
        }
    }
}
