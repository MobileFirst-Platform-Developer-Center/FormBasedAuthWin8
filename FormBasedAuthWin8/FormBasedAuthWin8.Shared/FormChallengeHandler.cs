/**
* Copyright 2015 IBM Corp.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
 
﻿using System;
using System.Collections.Generic;
using System.Text;
using IBM.Worklight;
using Windows.UI.Popups;
using Windows.ApplicationModel.Core;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Windows.UI.Core;
using Windows.UI;

namespace FormBasedAuthWin8
{
    class FormChallengeHandler : ChallengeHandler
        {
            String username;
            String password;

            public FormChallengeHandler() : base("SampleAppRealm")
            {

            }

            public override void handleChallenge(JObject response)
            {
                Debug.WriteLine("In Formchallengehandler handleChallenge");

                MainPage._this.showChallenge();

            }

            public void sendResponse(String username, String password)
            {
                Debug.WriteLine("In FormChallengehandler sendResponse");
                Dictionary<String, String> parms = new Dictionary<String, String>();

                parms.Add("j_username", username);
                parms.Add("j_password", password);

                submitLoginForm("j_security_check", parms, null, 0, "post");

            }

            public override bool isCustomResponse(WLResponse response)
            {
                Debug.WriteLine("In FormChallengeHandler isCustomResponse");

                if (response == null || response.getResponseText() == null || !response.getResponseText().Contains("j_security_check"))
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

            public override void onSuccess(WLResponse resp)
            {
                Debug.WriteLine("Challenge handler success");

                MainPage._this.hideChallenge();

                submitSuccess(resp);
            }

            public override void onFailure(WLFailResponse failResp)
            {
                Debug.WriteLine("Challenge handler failure ");
            }
    }
}
