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
        public void TestReturnsCorrectType () {
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

            Object update = client.ExecuteMove (new Board (list));

            if (!(update is GameStateUpdate)) {
                throw new AssertFailedException ("Should have succeeded");
            }
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

            if (!(update is ErrorResponse)) {
                throw new AssertFailedException ("Should have errored");
            }
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

            if (!(update is ErrorResponse)) {
                throw new AssertFailedException ("Should have errored");
            }
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

            if (!(update is ErrorResponse)) {
                throw new AssertFailedException ("Should have errored");
            }
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

            if (!(update is ErrorResponse)) {
                throw new AssertFailedException ("Should have errored");
            }
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

            if (!(update is ErrorResponse)) {
                throw new AssertFailedException ("Should have errored");
            }
        }

        [TestMethod]
        public void TestErrorsForNull () {
            ServiceClientCredentials serviceClientCredentials = new TokenCredentials ("FakeTokenValue");
            TicTacToeSDKClient client = new TicTacToeSDKClient (new Uri ("https://localhost:44305"), serviceClientCredentials);

            Object update = client.ExecuteMove (new Board (null));

            if (!(update is ErrorResponse)) {
                throw new AssertFailedException ("Should have errored");
            }
        }
    }
}
