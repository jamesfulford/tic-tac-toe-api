using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToeSDK;
using TicTacToeSDK.Models;

namespace TicTacToeFunctionalTests {
    [TestClass]
    public class TestHappyPaths {
        [TestMethod]
        public void TestPlaysXFirst () {
            List<string> list = new List<string> {
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            GameStateUpdate update = (GameStateUpdate) client.ExecuteMove (new Board (list));

            Assert.AreEqual<string> (update.AzurePlayerSymbol, "X");
            Assert.AreEqual<string> (update.Winner, "inconclusive");
            Assert.IsNull (update.WinPositions);
        }

        public void TestDetectsIfIsPlayerO () {
            List<string> list = new List<string> {
                "?",
                "?",
                "?",
                "?",
                "X",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            GameStateUpdate update = (GameStateUpdate) client.ExecuteMove (new Board (list));

            Assert.AreEqual<string> (update.AzurePlayerSymbol, "O");
            Assert.AreEqual<string> (update.Winner, "inconclusive");
            Assert.IsNull (update.WinPositions);
        }

        public void TestDetectsIfIsPlayerX () {
            List<string> list = new List<string> {
                "?",
                "?",
                "?",
                "?",
                "O",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            GameStateUpdate update = (GameStateUpdate) client.ExecuteMove (new Board (list));

            Assert.AreEqual<string> (update.AzurePlayerSymbol, "X");
            Assert.AreEqual<string> (update.Winner, "inconclusive");
            Assert.IsNull (update.WinPositions);
        }

        [TestMethod]
        public void TestDetectsXWin () {
            List<string> list = new List<string> {
                "X",
                "O",
                "?",
                "?",
                "X",
                "?",
                "?",
                "O",
                "X"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            GameStateUpdate update = (GameStateUpdate) client.ExecuteMove (new Board (list));

            Assert.AreEqual<string> (update.Winner, "X");
            Assert.AreEqual<List<int>> ((List<int>) update.WinPositions, new List<int> { 0, 4, 8 });
        }

        public void TestDetectsOWin () {
            List<string> list = new List<string> {
                "X",
                "?",
                "O",
                "?",
                "O",
                "?",
                "O",
                "?",
                "X"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            GameStateUpdate update = (GameStateUpdate) client.ExecuteMove (new Board (list));

            Assert.AreEqual<string> (update.Winner, "O");
            Assert.AreEqual<List<int>> ((List<int>) update.WinPositions, new List<int> { 2, 4, 6 });
        }

        public void TestDetectsTie () {
            // X O X
            // X O X
            // O X O
            List<string> list = new List<string> {
                "X",
                "O",
                "X",
                "X",
                "O",
                "X",
                "O",
                "X",
                "O"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            GameStateUpdate update = (GameStateUpdate) client.ExecuteMove (new Board (list));

            Assert.AreEqual<string> (update.Winner, "tie");
            Assert.IsNull(update.WinPositions);
        }
    }

    [TestClass]
    public class TestNegativePaths {
        [TestMethod]
        public void TestErrorsForWrongLength () {
            List<string> list = new List<string> {
                // 8, not 9
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (list));

            Assert.IsTrue (update is ErrorResponse);
        }

        [TestMethod]
        public void TestErrorsForTurnSkipping () {
            List<string> list = new List<string> {
                "X", // 3 X's
                "X",
                "X",
                "O", // 1 O's
                "?",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (list));

            Assert.IsTrue (update is ErrorResponse);
        }

        [TestMethod]
        public void TestErrorsForLowerCaseSymbols () {
            List<string> list = new List<string> {
                "x", // nope
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (list));

            Assert.IsTrue (update is ErrorResponse);
        }

        [TestMethod]
        public void TestErrorsForMultiCharacterSymbols () {
            List<string> list = new List<string> {
                "XX", // nope
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (list));

            Assert.IsTrue (update is ErrorResponse);
        }

        [TestMethod]
        public void TestErrorsForNonSymbol () {
            List<string> list = new List<string> {
                "A", // nope
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?",
                "?"
            };
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (list));

            Assert.IsTrue (update is ErrorResponse);
        }

        [TestMethod]
        public void TestErrorsForNull () {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (null));

            Assert.IsTrue (update is ErrorResponse);
        }
    }
}
