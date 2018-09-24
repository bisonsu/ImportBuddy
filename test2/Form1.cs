using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace test2
{

    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
            button4.Enabled = false; //This is to initially disable the Browse button of the output file. Only enabled if user selects checkBox5
            textBox1.ReadOnly = true; //This disables the user from typing in the textBox where the import file path is supposed to display
            textBox2.ReadOnly = true; //This disables the user from typing in the textBox where the output file path is supposed to display
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        bool noFileFormat; //This initializes the variable to confirm if a file format was selected
        string outputFile;
        string FileNameOnly { get; set; }

        public void button1_Click(object sender, EventArgs e)
        {
            string fileNameOnly;
            noFileFormat = false; //Initially set to match that a file format is not selected when the application is launched
            VerifyFileFormat(); //Method to verify a file format is selected

            if (!noFileFormat) //If there is a file format selected, fileFormat is set to false and will continue with this if statement. If no file format selected, unable to continue.
            {

                OpenFileDialog ofd = new OpenFileDialog(); //procedure to browse available files
                ofd.Filter = "All Files (*.*|*.*"; //allows any file to be selected
                ofd.RestoreDirectory = true; //restores directory you last selected
                if(ofd.ShowDialog() == DialogResult.OK) //if you select a file, code continue. If not, nothing is done.
                {
                    string fileName = ofd.FileName; //initializes fileName as the full path of the selected file
                    if(string.IsNullOrWhiteSpace(textBox1.Text)) //This checks if the output file path is selected. If selected, this is skipped
                    {
                        textBox1.Text = System.IO.Path.GetDirectoryName(ofd.FileName); //the field defaults to the same path as the import file's path if no path is already entered.
                    }
                    textBox2.Text = fileName; //populates the full path of the import file
                    FileNameOnly = Path.GetFileName(fileName); //initializes only the file name to this variable
                    

                }

            }
        }



        private void VerifyFileFormat()
        {
            bool EZR; //initializes EZRoute for file format. To be used to run validation/MIU extration for EZRoute format
            bool EZP; //Initializes V2 or V4 for file format. To be used to run validation/MIU extraction for V2 or V4 format
            

            //Verify the File Format was selected
            if (radioButton1.Checked)
            {
                EZR = true; //EZRoute format selected
            }

            if (radioButton2.Checked)
            {
                EZP = true; //V2 or V4 format selected
            }

            if (!radioButton1.Checked & !radioButton2.Checked)
            {
                noFileFormat = true; //Changes value to true and indicates no file format was selected
                MessageBox.Show("You did not select a file format! Unable to continue."); //Message to indicate unable to continue
                return; //returns to main form. no actions performed

            }
        }

        private void button4_Click(object sender, EventArgs e) //allows user to specify where they would like the output file to be
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog(); 
            fbd.Description = "Choose location for Output file";

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) //allows user to enable to browse for the output file option
        {
            if (checkBox5.Checked == true)
            {
                button4.Enabled = true;
            }

            if (checkBox5.Checked == false)
            {
                button4.Enabled = false;
                //textBox1.Text = "";
            }
        }

        Regex Alpha = new Regex("^[a-zA-Z]*$");
        Regex Numeric = new Regex("^[a-zA-Z0-9]*$");
        Regex AlphaNumeric = new Regex("^[a-zA-Z0-9 ]*$");
        Regex AorI = new Regex("^[aAiI ]*$");
        Regex RecordTypeStatus = new Regex("^[a-zA-Z ]*$");
        Regex DisplayFields = new Regex("^[a-zA-Z0-9()* :;,&.'!@#=~/\"-]*$");



        public void button3_Click(object sender, EventArgs e) //starts the file validation/MIU extraction
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Must select file to begin.");
                return;
            }

            if (checkBox1.Checked == false & checkBox2.Checked == false)
            {
                MessageBox.Show("Must select output option to begin.");
            }

            if (checkBox1.Checked == true)
            {
                
                //Perform validation of known file format issues
                //Confirm each line ends with crlf
                //Confirm each date is valid
                //Confirm each field matches alphanumeric properties

                if(radioButton1.Checked == true)//EZR
                {
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    outputFile = textBox1.Text + "\\" + FileNameOnly + ".datavalidation";
                    System.IO.File.WriteAllText(outputFile, string.Empty);
                    StreamWriter sw = new StreamWriter(File.OpenWrite(outputFile));
                    
                    int counter = 1;
                    string line;

                    // Read the file and display it line by line
                    StreamReader file = new StreamReader(textBox2.Text);
                    while ((line = file.ReadLine()) != null)
                    {
                        int lineLength = line.Length;
                        bool lineLengthMet = false;
                        string lineLengthString;

                        System.Console.WriteLine(line);
                        string routeId = line.Substring(0,10).Trim();
                        string walkSequence = line.Substring(10,4).Trim();
                        string pageNumber = line.Substring(14,4).Trim();
                        string readSequence = line.Substring(18,2).Trim();
                        string handheldId = line.Substring(20,6).Trim();
                        string readDirection = line.Substring(26,1).Trim();
                        string noOfDials = line.Substring(27,1).Trim();
                        string idExpected = line.Substring(28,13).Trim();
                        string idCaptured = line.Substring(41,13).Trim();
                        string idOverride = line.Substring(54,13).Trim();
                        string decimalLocation = line.Substring(67,1).Trim();
                        string meterReading = line.Substring(68,10).Trim();
                        string readingOverride = line.Substring(78,10).Trim();
                        string highReadingLimit = line.Substring(88,10).Trim();
                        string lowReadingLimit = line.Substring(98,10).Trim();
                        string dateToRead = line.Substring(108,6).Trim();
                        string dateToExport = line.Substring(114,6).Trim();
                        string notes = line.Substring(120,8).Trim();
                        string locationCode = line.Substring(128,2).Trim();
                        string meterReaderCode = line.Substring(130,2).Trim();
                        string recordType = line.Substring(132,2).Trim();
                        string recordStatus = line.Substring(134,1).Trim(); 
                        string date = line.Substring(135,6).Trim();
                        string time = line.Substring(141,6).Trim();
                        string typeOfReading = line.Substring(147,1).Trim();
                        string networkNumber = line.Substring(148,2).Trim();
                        string readAttempts = line.Substring(150,1).Trim();
                        string userCharacters = line.Substring(151,7).Trim();
                        string manufacturer = line.Substring(158,1).Trim();
                        string activeInactive = line.Substring(159,1);
                        string typeOfMeter = line.Substring(160,1).Trim();
                        string readFailCode = line.Substring(161,1).Trim();
                        string prevReading = line.Substring(162,10).Trim();
                        string prevReadingDate = line.Substring(172,6).Trim();
                        string display11 = line.Substring(178,24).Trim();
                        string display12 = line.Substring(202,24).Trim();
                        string display13 = line.Substring(226,24).Trim();
                        string display14 = line.Substring(250,24).Trim();
                        string display21 = line.Substring(274,24).Trim();
                        string display22 = line.Substring(298,24).Trim();
                        string display23 = line.Substring(322,24).Trim();
                        string display24 = line.Substring(346,24).Trim();
                        string display25 = line.Substring(370,24).Trim();
                        string display26 = line.Substring(394,24).Trim();
                        string display27 = line.Substring(418,24).Trim();
                        string display28 = line.Substring(442,24).Trim();
                        string display2OpCode = line.Substring(466,1).Trim();
                        string utilityField = line.Substring(467,40).Trim();


                        var requiredAlphaNumeric = new List<string> { routeId, readDirection, noOfDials};
                        var requiredAorI = new List<string> { activeInactive };
                        var requiredNumber = new List<string> { pageNumber, readSequence, decimalLocation, highReadingLimit, lowReadingLimit };
                        var notRequiredAlphaNumeric = new List<string> { locationCode, typeOfMeter, idExpected , manufacturer};
                        var notrequiredNumber = new List<string> { walkSequence, dateToRead, dateToExport, prevReading, prevReadingDate };
                        var notRequiredBlank = new List<string> { handheldId, idCaptured, idOverride, meterReading, readingOverride, notes, meterReaderCode, date, time, typeOfReading, networkNumber, readAttempts, userCharacters, readFailCode, display2OpCode };
                        var notRequiredDisplayFields = new List<string> { utilityField, display11, display12, display13, display14, display21, display22, display23, display24, display25, display26, display27, display28 };
                        var recordTypeStatus = new List<string> { recordType, recordStatus };

                        int countOfList = 0;
                        string variableName = "";
                        foreach (string item in requiredAlphaNumeric)
                        {
                            countOfList++;
                            if(countOfList == 1)
                            {
                                variableName = "routeId";
                            }
                            if (countOfList == 2)
                            {
                                variableName = "readDirection";
                            }
                            if (countOfList == 3)
                            {
                                variableName = "noOfDials";
                            }

                            if (AlphaNumeric.IsMatch(item))
                            {
                                if(item != "")
                                {
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is valid");
                                }
                                else
                                {
                                    sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not valid");
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not valid");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not alphanumeric");
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not alphanumeric");
                            }

                        }

                        foreach (string item in requiredAorI)
                        {
                            variableName = "Active or Inactive";
                            if (AorI.IsMatch(item))
                            {
                                if (item == "A" | item == "I" | item == " ")
                                {
                                    //MessageBox.Show("Line: " + counter + " -" + variableName + " " + item + " is valid");
                                }
                                else
                                {
                                    sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not valid");
                                    //MessageBox.Show("Line: " + counter + " -" + variableName + " " + item + " is not valid");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " -" + variableName + " " + item + " is not A, I, or blank");
                                //MessageBox.Show("Line: " + counter + " -" + variableName + " " + item + " is not A or I");
                            }
                        }
                        countOfList = 0;
                        foreach (string item in requiredNumber)
                        {
                            countOfList++;
                            if (countOfList == 1)
                            {
                                variableName = "pageNumber";
                            }
                            if (countOfList == 2)
                            {
                                variableName = "readSequence";
                            }
                            if (countOfList == 3)
                            {
                                variableName = "decimalLocation";
                            }
                            if (countOfList == 4)
                            {
                                variableName = "highReadingLimit";
                            }
                            if (countOfList == 5)
                            {
                                variableName = "lowReadingLimit";
                            }
                            if (Numeric.IsMatch(item))
                            {
                                if (item != "")
                                {
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is valid");
                                }
                                else
                                {
                                    sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not valid");
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not valid");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not numeric");
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not numeric");
                            }

                        }
                        countOfList = 0;
                        foreach (string item in notRequiredAlphaNumeric)
                        {
                            countOfList++;
                            if (countOfList == 1)
                            {
                                variableName = "locationCode";
                            }
                            
                            if (countOfList == 2)
                            {
                                variableName = "typeOfMeter";
                            }
                            if (countOfList == 3)
                            {
                                variableName = "idExpected";
                            }
                            if (countOfList == 4)
                            {
                                variableName = "idExpected";
                            }

                            if (AlphaNumeric.IsMatch(item))
                            {
                                if (item != "")
                                {
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is valid");
                                }
                                else
                                {
                                    //sw.Write("Line: " + counter + " - " + variableName + " " + item + " is null\r\n");
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is null");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not alphanumeric");
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not alphanumeric");
                            }

                        }
                        countOfList = 0;
                        foreach (string item in notrequiredNumber)
                        {
                            countOfList++;
                            if (countOfList == 1)
                            {
                                variableName = "walkSequence";
                            }
                            if (countOfList == 2)
                            {
                                variableName = "dateToRead";
                            }
                            if (countOfList == 3)
                            {
                                variableName = "dateToExport";
                            }
                            if (countOfList == 4)
                            {
                                variableName = "prevReading";
                            }
                            if (countOfList == 5)
                            {
                                variableName = "prevReadingDate";
                            }
                            if (Numeric.IsMatch(item))
                            {
                                if (item != "")
                                {
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is valid");
                                }
                                else
                                {
                                    //sw.Write("Line: " + counter + " - " + variableName + " " + item + " is null\r\n");
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is null");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not numeric");
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not numeric");
                            }
                        }
                        countOfList = 0;
                        foreach (string item in notRequiredBlank)
                        {
                            countOfList++;
                            if (countOfList == 1)
                            {
                                variableName = "handheldId";
                            }
                            if (countOfList == 2)
                            {
                                variableName = "idCaptured";
                            }
                            if (countOfList == 3)
                            {
                                variableName = "idOverride";
                            }
                            if (countOfList == 4)
                            {
                                variableName = "meterReading";
                            }
                            if (countOfList == 5)
                            {
                                variableName = "readingOverride";
                            }
                            if (countOfList == 6)
                            {
                                variableName = "notes";
                            }
                            if (countOfList == 7)
                            {
                                variableName = "meterReaderCode";
                            }
                            if (countOfList == 8)
                            {
                                variableName = "date";
                            }
                            if (countOfList == 9)
                            {
                                variableName = "time";
                            }
                            if (countOfList == 10)
                            {
                                variableName = "typeOfReading";
                            }
                            if (countOfList == 11)
                            {
                                variableName = "networkNumber";
                            }
                            if (countOfList == 12)
                            {
                                variableName = "readAttempts";
                            }
                            if (countOfList == 13)
                            {
                                variableName = "userCharacters";
                            }
                            if (countOfList == 14)
                            {
                                variableName = "readFailCode";
                            }
                            if (countOfList == 15)
                            {
                                variableName = "display2OpCode";
                            }
                            if (item != "")
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " should be null");
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is valid");
                            }
                            else
                            {
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is null");
                            }
                        }
                        countOfList = 0;
                        foreach (string item in notRequiredDisplayFields)
                        {
                            countOfList++;
                            if (countOfList == 1)
                            {
                                variableName = "utilityField";
                            }
                            if (countOfList == 2)
                            {
                                variableName = "display11";
                            }
                            if (countOfList == 3)
                            {
                                variableName = "display12";
                            }
                            if (countOfList == 4)
                            {
                                variableName = "display13";
                            }
                            if (countOfList == 5)
                            {
                                variableName = "display14";
                            }
                            if (countOfList == 6)
                            {
                                variableName = "display21";
                            }
                            if (countOfList == 7)
                            {
                                variableName = "display22";
                            }
                            if (countOfList == 8)
                            {
                                variableName = "display23";
                            }
                            if (countOfList == 9)
                            {
                                variableName = "display24";
                            }
                            if (countOfList == 10)
                            {
                                variableName = "display25";
                            }
                            if (countOfList == 11)
                            {
                                variableName = "display26";
                            }
                            if (countOfList == 12)
                            {
                                variableName = "display27";
                            }
                            if (countOfList == 13)
                            {
                                variableName = "display28";
                            }
                            if (DisplayFields.IsMatch(item))
                            {
                                if (item != "")
                                {
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is valid");
                                }
                                else
                                {
                                    //sw.Write("Line: " + counter + " - " + variableName + " " + item + " is null\r\n");
                                    //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is null");
                                }
                                if(item == "" && variableName == "display11")
                                {
                                    sw.WriteLine("Display 11 is not used. It is recommended to have the address field populated here.");
                                }
                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not alphanumeric");
                                //MessageBox.Show("Line: " + counter + " - " + variableName + " " + item + " is not numeric");
                            }
                        }
                        countOfList = 0;
                        foreach (string item in recordTypeStatus)
                        {
                            countOfList++;
                            if (countOfList == 1)
                            {
                                variableName = "recordType";
                            }
                            if (countOfList == 1)
                            {
                                variableName = "recordStatus";
                            }
                            if(RecordTypeStatus.IsMatch(item))
                            {

                            }
                            else
                            {
                                sw.WriteLine("Line: " + counter + " - " + variableName + " " + item + " is not valid");
                            }

                        }

                            counter++;//Increases the line number for logging
                    }

                    counter--; //Outputs the correct number of lines in the next line of code

                    sw.WriteLine("There were {0} lines.", counter);
                    watch.Stop();
                    var elapsedMS = watch.ElapsedMilliseconds;
                    sw.WriteLine("Time taken: {0}ms", elapsedMS);
                    sw.Dispose();
                    sw.Close();

                    MessageBox.Show("Validation file complete. Location of validation file is: " + outputFile);
                    // Suspend the screen.  
                       
                }

                if (radioButton2.Checked == true)
                {
                    //Do for EZP
                    outputFile = textBox1.Text + "\\" + FileNameOnly + ".datavalidation";
                    MessageBox.Show(outputFile);
                }

            }

            if (checkBox2.Checked == true)
            {
                
                outputFile = textBox1.Text + "\\" + FileNameOnly + ".csv";
                MessageBox.Show(outputFile);

            }

            //Append to log file the Errors received with Import Buddy
            //Append to log file a timestamp with each entry
            //Append to log file Errors received with the import file
            //Save a copy of the import file to a folder inside Import Buddy directory
            //Reference the import file that was used
            //Search for number of times run and increment number on next file entry
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
