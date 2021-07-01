/*
https://ru.wikipedia.org/wiki/%D0%A8%D0%B8%D1%84%D1%80_%D0%92%D0%B8%D0%B6%D0%B5%D0%BD%D0%B5%D1%80%D0%B0#cite_ref-10
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vigenere
{
    public partial class Form1 : Form
    {
        VigenereCipher cipher;

        public string vigenere(string s, string key, bool decode)
        {
            string result = "";

            int min_ascii = 33; // Минимальная граница алфавита в ascii
            int max_ascii = 126; // Максимальная граница алфавита в ascii

            int n = max_ascii - min_ascii + 1; // Количество символов алфавита

            string long_key = ""; // Ключ повторенный подрят что бы сровняться или быть больше шифруемой строки
            while (long_key.Length < s.Length)
            {
                long_key += key;
            }
            
            for (int i = 0; i < s.Length; i++)
            {
                int new_char;

                if (!decode) // Кодирование
                {
                    new_char = ((byte)s[i] + (byte)long_key[i]) % n + min_ascii;
                }
                else // Декодирование
                {
                    new_char = ((byte)s[i] + n - (byte)long_key[i]) % n - min_ascii;
                }

                result += (char)new_char;
            }

            return result;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Кодирование
            
            //textBox3.Text = cipher.Encrypt(textBox2.Text, key_textBox.Text) + Environment.NewLine;
            
            textBox3.Text += vigenere(textBox2.Text, key_textBox.Text, false);
            
            textBox5.Text = vigenere(textBox2.Text, key_textBox.Text, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Декодирование

            //textBox4.Text = cipher.Decrypt(textBox5.Text, key_textBox.Text) + Environment.NewLine;
            textBox4.Text = vigenere(textBox5.Text, key_textBox.Text, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string book = "";
            for (int i = 30; i <= 126; i++)
            {
                book += (char)i;
            }

            cipher = new VigenereCipher(book);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = "ABCD";
            byte b = (byte)s[0];
            textBox4.Text = b.ToString();
        }
    }

    public class VigenereCipher
    {
        const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        readonly string letters;

        public VigenereCipher(string alphabet = null)
        {
            letters = string.IsNullOrEmpty(alphabet) ? defaultAlphabet : alphabet;
        }

        //генерация повторяющегося пароля
        private string GetRepeatKey(string s, int n)
        {
            var p = s;
            while (p.Length < n)
            {
                p += p;
            }

            return p.Substring(0, n);
        }

        private string Vigenere(string text, string password, bool encrypting = true)
        {
            var gamma = GetRepeatKey(password, text.Length);
            var retValue = "";
            var q = letters.Length;

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = letters.IndexOf(text[i]);
                var codeIndex = letters.IndexOf(gamma[i]);
                if (letterIndex < 0)
                {
                    //если буква не найдена, добавляем её в исходном виде
                    retValue += text[i].ToString();
                }
                else
                {
                    retValue += letters[(q + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % q].ToString();
                }
            }

            return retValue;
        }

        //шифрование текста
        public string Encrypt(string plainMessage, string password)
            => Vigenere(plainMessage, password);

        //дешифрование текста
        public string Decrypt(string encryptedMessage, string password)
            => Vigenere(encryptedMessage, password, false);
    }
}
