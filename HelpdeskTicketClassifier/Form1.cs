using System.Net.Http; //Let this app send an online request to OpenAI
using System.Text;
using System.Text.Json; //Let this app read JSON data from OpenAI’s response

namespace HelpdeskTicketClassifier
{
    public partial class Form1 : Form
    {
        private class AiClassificationResult
        {
            public string Category { get; set; } = "";
            public string Priority { get; set; } = "";
            public string Team { get; set; } = "";
            public string Summary { get; set; } = "";
            public string Response { get; set; } = "";
            public string Confidence { get; set; } = "";
            //Instead of having loose random values everywhere,
            //we put the OpenAI result into one organized object.
        }
        private string GetOpenAiApiKey()
        {
            return Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? "";
        }
        private async Task<AiClassificationResult?> ClassifyWithOpenAiAsync(string issue)
        {
            string apiKey = GetOpenAiApiKey();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return null;
            }

            string prompt =
                "You are an IT helpdesk ticket classifier. " +
                "Classify the user's issue into a support category, priority, support team, summary, suggested response, and confidence. " +
                "Return only valid JSON with these exact fields: category, priority, team, summary, response, confidence. " +
                "Confidence should be a percentage string like 85%. " +
                "User issue: " + issue;

            using HttpClient client = new HttpClient();
            //Create a small internet request tool so your app can talk to OpenAI.
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);

            var requestBody = new
            {
                model = "gpt-4o-mini",
                input = prompt
            };

            string jsonBody = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponse = await client.PostAsync("https://api.openai.com/v1/responses", content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            string responseJson = await httpResponse.Content.ReadAsStringAsync();

            using JsonDocument document = JsonDocument.Parse(responseJson);

            string aiText = document
                .RootElement
                .GetProperty("output")[0]
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString() ?? "";

            AiClassificationResult? result = JsonSerializer.Deserialize<AiClassificationResult>(
                aiText,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return result;
        }
        //“Look for a saved key called OPENAI_API_KEY. If it exists, use it. If not, return empty text.”
        string currentTicketId = "";
        string currentCreatedTime = "";
        string currentCategory = "";
        string currentPriority = "";
        string currentConfidence = "";
        string currentTeam = "";
        string currentSummary = "";
        string currentResponse = "";
        string currentClassifiedBy = "";
        string currentAiStatus = "";

        bool currentTicketSaved = false;//The current ticket has not been saved yet.

        private string CsvEscape(string value)
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
        public Form1()
        {
            InitializeComponent();

            this.Text = "Hybrid AI Helpdesk Ticket Classifier";

            TicketHistory.Columns.Add("TicketId", "Ticket ID");
                TicketHistory.Columns.Add("CreatedTime", "Created Time");
                TicketHistory.Columns.Add("Category", "Category");
                TicketHistory.Columns.Add("Priority", "Priority");
                TicketHistory.Columns.Add("Confidence", "Confidence");
                TicketHistory.Columns.Add("ClassifiedBy", "Classified By");
                TicketHistory.Columns.Add("AiStatus", "AI Status");
                TicketHistory.Columns.Add("Team", "Team");
                
                TicketHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                TicketHistory.ReadOnly = true;
                TicketHistory.AllowUserToAddRows = false;
                TicketHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadTicketsFromCsv();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private async void btnClassify_Click(object sender, EventArgs e)
        {
            string issue = txtIssue.Text.ToLower();
            if (string.IsNullOrWhiteSpace(issue))
            {
                txtResult.Text = "Please enter a user issue first.";
                return;
            }

            string category = "";
            string priority = "";
            string team = "";
            string summary = "";
            string response = "";
            string confidence = "";
            string classifiedBy = "Rule-Based";
            string aiStatus = "Not needed";//this is every new classification starts with
            if (issue.Contains("phishing") || issue.Contains("suspicious") || issue.Contains("scam") || issue.Contains("virus") || issue.Contains("malware"))
            {
                category = "Security / Phishing Issue";
                priority = "High";
                team = "Security Team";
                summary = "User reported a possible security threat such as phishing, malware, or a suspicious message.";
                response = "Please do not click any links or download attachments. Report the message to the security team and change your password if you shared any information.";
                confidence = "95%";
            }
            else if (issue.Contains("payroll") || issue.Contains("hr") || issue.Contains("timesheet") || issue.Contains("workday") || issue.Contains("benefits"))
            {
                category = "Payroll / HR System Issue";
                priority = "Medium";
                team = "HR Systems Support";
                summary = "User is experiencing an issue related to payroll, timesheets, benefits, or an HR system.";
                response = "Please confirm the employee ID, affected pay period or timesheet date, and any error message shown. HR Systems Support can review the account or payroll system settings.";
                confidence = "90%";
            }
            else if (issue.Contains("login") || issue.Contains("password") || issue.Contains("reset") || issue.Contains("workday"))
            {
                category = "Login / Account Access";
                priority = "Medium";
                team = "IT Support";
                summary = "User cannot log into an account or system and may need password or session troubleshooting.";
                response = "Please try clearing browser cookies or resetting your password. If the issue continues, IT can check your SSO settings.";
                confidence = "90%";
            }

            else if (issue.Contains("wifi") || issue.Contains("internet") || issue.Contains("network") || issue.Contains("connection"))
            {
                category = "Network Issue";
                priority = "Medium";
                team = "IT Support";
                summary = "User is experiencing a network or internet connection issue.";
                response = "Please restart the device, check the WiFi connection, and verify that the network is available. If the issue continues, IT can troubleshoot the connection.";
                confidence = "88%";
            }
            else if (issue.Contains("email") || issue.Contains("outlook") || issue.Contains("inbox") || issue.Contains("send email") || issue.Contains("receive email"))
            {
                category = "Email / Outlook Issue";
                priority = "Medium";
                team = "IT Support";
                summary = "User is experiencing an email-related issue, such as sending, receiving, or accessing messages.";
                response = "Please check the internet connection, restart Outlook, and confirm whether the user can access email through the web version. If the issue continues, IT can check the mailbox settings.";
                confidence = "88%";
            }
            else if (issue.Contains("install") || issue.Contains("installation") || issue.Contains("software") || issue.Contains("app") || issue.Contains("application"))
            {
                category = "Software Installation Issue";
                priority = "Medium";
                team = "Application Support";
                summary = "User is experiencing an issue installing or accessing software.";
                response = "Please confirm the software name, check whether the user has permission to install it, and try restarting the device. If the issue continues, Application Support can review the installation or access settings.";
                confidence = "85%";
            }
            else if (issue.Contains("laptop") || issue.Contains("screen") || issue.Contains("keyboard") || issue.Contains("mouse") || issue.Contains("printer"))
            {
                category = "Hardware Issue";
                priority = "Medium";
                team = "IT Support";
                summary = "User is experiencing a hardware-related issue with a device or peripheral.";
                response = "Please check the device connections, restart the device, and confirm whether the hardware is powered on. If the issue continues, IT can inspect or replace the equipment.";
                confidence = "85%";
            }
            else if (issue.Contains("email") || issue.Contains("outlook") || issue.Contains("inbox") || issue.Contains("send email") || issue.Contains("receive email"))
            {
                category = "Email / Outlook Issue";
                priority = "Medium";
                team = "IT Support";
                summary = "User is experiencing an email-related issue, such as sending, receiving, or accessing messages.";
                response = "Please check the internet connection, restart Outlook, and confirm whether the user can access email through the web version. If the issue continues, IT can check the mailbox settings.";
                confidence = "88%";
            }
            else
            {
                category = "General Support";
                priority = "Low";
                team = "Helpdesk";
                summary = "User reported a general support issue.";
                response = "Please collect more details from the user and route the ticket to the appropriate support team.";
                confidence = "50%";
            }
            if (confidence == "50%")
            {
                AiClassificationResult? aiResult = await ClassifyWithOpenAiAsync(issue);
                //Send the user’s issue to OpenAI and wait for the AI classification result.
                if (aiResult != null)
                {
                    category = aiResult.Category;
                    priority = aiResult.Priority;
                    team = aiResult.Team;
                    summary = aiResult.Summary;
                    response = aiResult.Response;
                    confidence = aiResult.Confidence;
                    classifiedBy = "OpenAI Fallback";
                }
                else
                {
                    classifiedBy = "Rule-Based Fallback";
                    aiStatus = "Attempted fallback";
                    summary = summary + " The system attempted AI fallback and safely returned the rule-based result.";
                }
            }
            string ticketId = "TCK-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string createdTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            currentTicketId = ticketId;
            currentCreatedTime = createdTime;
            currentCategory = category;
            currentPriority = priority;
            currentConfidence = confidence;
            currentTeam = team;
            currentSummary = summary;
            currentResponse = response;
            currentClassifiedBy = classifiedBy;
            currentAiStatus = aiStatus;

            currentTicketSaved = false; //every new classified ticket starts as not saved yet

            txtResult.Text =
                "Ticket ID: " + ticketId + Environment.NewLine +
                "Created Time: " + createdTime + Environment.NewLine +
                "Category: " + category + Environment.NewLine +
                "Priority: " + priority + Environment.NewLine +
                "Confidence: " + confidence + Environment.NewLine +
                 "Classified By: " + classifiedBy + Environment.NewLine +
                 "AI Status: " + (classifiedBy == "OpenAI Fallback" ? "Used" : classifiedBy == "Rule-Based Fallback" ? "Attempted fallback" : "Not needed") + Environment.NewLine +
                "Suggested Team: " + team + Environment.NewLine +
                "Summary: " + summary + Environment.NewLine +
                "Suggested Response: " + response;

            TicketHistory.Rows.Add(ticketId, createdTime, category, priority, confidence, classifiedBy, aiStatus, team);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtIssue.Clear();
            txtResult.Clear();
            //txtIssue.Focus();

            currentTicketId = "";
            currentCreatedTime = "";
            currentCategory = "";
            currentPriority = "";
            currentConfidence = "";
            currentTeam = "";
            currentSummary = "";
            currentResponse = "";
            currentClassifiedBy = "";
            currentAiStatus = "";

            currentTicketSaved = false;//Forget the current ticket. I want to start fresh

            txtIssue.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtResult.Text))
            {
                MessageBox.Show("Please classify a ticket before saving.");
                return;
            }

            if (currentTicketSaved)
            {
                MessageBox.Show("This ticket has already been saved.");
                return;
            }

            string filePath = "tickets.csv";

            bool fileExists = File.Exists(filePath);

            if (!fileExists)
            {
                File.AppendAllText(filePath, "Ticket ID,Created Time,Category,Priority,Confidence,Classified By,AI Status,Suggested Team,Summary,Suggested Response" + Environment.NewLine);
            }

            string csvLine =
     CsvEscape(currentTicketId) + "," +
     CsvEscape(currentCreatedTime) + "," +
     CsvEscape(currentCategory) + "," +
     CsvEscape(currentPriority) + "," +
     CsvEscape(currentConfidence) + "," +
     CsvEscape(currentClassifiedBy) + "," +
     CsvEscape(currentAiStatus) + "," +
     CsvEscape(currentTeam) + "," +
     CsvEscape(currentSummary) + "," +
     CsvEscape(currentResponse);

            File.AppendAllText(filePath, csvLine + Environment.NewLine);

            currentTicketSaved = true;//After saving successfully, mark it as saved

            MessageBox.Show("Ticket saved successfully.");
        }
        private void LoadTicketsFromCsv() // this method lets your app remember old saved tickets after you close and reopen the app.
        {
            string filePath = "tickets.csv"; //The file I want to read is called tickets.csv

            if (!File.Exists(filePath))
            {
                return;
            }

            string[] lines = File.ReadAllLines(filePath);//This reads all lines from tickets.csv

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');

                for (int j = 0; j < values.Length; j++)
                {
                    values[j] = values[j].Trim('"');
                }
                //Go through every value one by one.
               // Remove the quotation marks from the beginning and end
                if (values.Length >= 8)
                {
                    TicketHistory.Rows.Add(
                        values[0], //values[0] = TCK-001
                        values[1], //values[1] = 05/16/2026
                        values[2], //values[2] = Login
                        values[3], //values[3] = Medium
                        values[4], //values[4] = 90%
                        values[5],
                        values[6],
                        values[7]
                    );
                }
            }
        }

    }
}
//setx OPENAI_API_KEY "api"