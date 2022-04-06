using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public enum FortranSymbolType 
    {   
       
        DataType, // тип данных
        Colon, // двоеточие
        Name, // имя массива
        OpenBrace, // открывающая скобка
        Argument, // параметр
        CloseBrace, // закрывающая скобка
        Eof, //Конец строки
        Error //Ошибка
    }

    public struct FortranSymbol
    {
        public FortranSymbolType Type;
        public string Value;
        public int Index;
        public FortranSymbol(FortranSymbolType type = FortranSymbolType.Eof, string value = " ", int index = 0)
        {
            Type = type;
            Value = value;
            Index = index;

        }
    }

    public class FortranScaner
    {
        private int _index = 0;
        private string _inputString;
        public int StringNum = 1;
        private int switcIndex = 0;
        private string result;
        private int temp_index_start;
        private int temp_index_last;
        private bool not_false_symbol = true;
        public FortranScaner(string inputString)
        {
            _inputString = inputString;
            _index = 0;
        }

        public void Reset(int temp)
        {
            _index = temp;
        }
        public void Set_start_point(int temp_index) // устанавливает начальный индекс символа
        {
            if (temp_index > 0)
                temp_index_start = temp_index - 1;
            else
                temp_index_start = 0;
        }
        public void Set_last_point(int number = 0) // устанавливает индекс после проверенного символа
        {
            temp_index_last = _index - number;
        }

        public FortranSymbol GetSymbol() // функция получения терминального символа
        {
            var value = String.Empty;
            int temp_index;
            while (_index < _inputString.Length && _inputString[_index] == ' ')
            {
                _index++;
            }
            temp_index = _index;
            while (!Eof)
            {
                value += _inputString[_index];
                _index++;

                if (Eof)
                {
                    break;
                }
                if (_inputString[_index] == '\n')
                {
                    StringNum++;
                }
                if (_inputString[_index] == ' ')
                {
                    Set_start_point(temp_index);
                    break;
                }


            }

            return CheckSymbolType(value);
        }

        public FortranSymbol CheckSymbolType(string value) // функция определения типа символа
        {
            
            if (GetColonSymbol(value).Type != FortranSymbolType.Error)
            {
                return GetColonSymbol(value);
            }
            if (GetDataTypeSymbol(value).Type != FortranSymbolType.Error)
            {
                return GetDataTypeSymbol(value);
            }
            if (GetCloseBraceSymbol(value).Type != FortranSymbolType.Error)
            {
                return GetCloseBraceSymbol(value);
            }
            if (GetArgumentSymbol(value).Type != FortranSymbolType.Error)
            {
                return GetArgumentSymbol(value);
            }
         
            if (GetNameSymbol(value).Type != FortranSymbolType.Error)
            {
               
                return GetNameSymbol(value);
            }
            if (GetOpenBraceSymbol(value).Type != FortranSymbolType.Error)
            {
                return GetOpenBraceSymbol(value);
            }

            return new FortranSymbol(FortranSymbolType.Error, value);
        }
        public string Recursive_Syntaxis(bool exit = true)
        {
            FortranSymbol symbol = GetSymbol();
            FortranSymbolType type;
            string message = "";

            switch (switcIndex) // выявление ошибок - соблюдение порядка и правильности написания символов
            {
                case 0:
                    
                    type = FortranSymbolType.DataType;
                    message = " Ошибка. Ожидался тип массива, Получено: {0} Позиция:{1}";
                    if (not_false_symbol)
                        Set_last_point();
                    break;
                case 1:
                   
                    type = FortranSymbolType.Colon;
                    message = " Ошибка. Ожидался символ ::, Получено: {0} Позиция:{1}";
                    if (not_false_symbol)
                        Set_last_point(1);
                    break;
                case 2:
                    
                    type = FortranSymbolType.Name;
                    message = " Ошибка. Неправильное объявление имени массива, Получено: {0} Позиция:{1}";
                    if (not_false_symbol)
                        Set_last_point();
                    break;
                case 3:
                    type = FortranSymbolType.OpenBrace;
                    message = " Ошибка. Ожидалась открывающая скобка, Получено: {0} Позиция:{1}";
                    if (not_false_symbol)
                        Set_last_point();
                    break;
                case 4:
                    type = FortranSymbolType.Argument;
                    message = " Ошибка. Неправильное объявление параметра массива, Получено: {0} Позиция:{1}";
                    if (not_false_symbol)
                        Set_last_point();
                    break;
                case 5:
                    type = FortranSymbolType.CloseBrace;
                    message = String.Format(" Ошибка. Ожидалась закрывающая скобка, Позиция:{0}", temp_index_last + 1);
                    if (not_false_symbol)
                        Set_last_point();
                    break;
               
                default:
                    type = FortranSymbolType.Eof;
                    break;
            }
            if (type == FortranSymbolType.Eof)
                return "";


            if (symbol.Type == type) // вывод в строку-результат позиции объекта 
            {
                result += ((int)symbol.Type).ToString() + String.Format(" Объект: {0}  Позиция:{1}", symbol.Value, symbol.Index) + "|";
                switcIndex++;
                exit = true;
                not_false_symbol = true;
                Set_last_point();
                Recursive_Syntaxis();

            }
            else
            {
                if (switcIndex != 0)
                    switcIndex++;

                if (exit)
                {
                    result += ((int)symbol.Type).ToString() + String.Format(message, symbol.Value, temp_index_last) + "|";
                    not_false_symbol = true;
                    Reset(temp_index_start);
                    return Recursive_Syntaxis(false);

                }

                if (!exit)
                {
                    if (switcIndex == 0)
                        switcIndex++;
                    else
                        switcIndex--;
                }
                not_false_symbol = false;
                if (type == FortranSymbolType.Eof)
                    return result;
                Reset(temp_index_last);

                Recursive_Syntaxis();

            }
            
            return result;
        }

        private bool Eof
        {
            get
            {
                return _index == _inputString.Length;
            }
        }

        private FortranSymbol GetOpenBraceSymbol(string value)
        {
            if (value == "(")
            {

                return new FortranSymbol(FortranSymbolType.OpenBrace, value, _index);

            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }

        private FortranSymbol GetNameSymbol(string value)
        {
            int i = 0;
            bool wrong_symbol = false;
            while (i < value.Length)
            {
                if (!IsLetter(value[i]) && !IsDigit(value[i]) || IsDigit(value[0]))
                {
                    wrong_symbol = true;
                }
                i++;
            }
            if (value != String.Empty && !wrong_symbol)
            {

                return new FortranSymbol(FortranSymbolType.Name, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }

        }
      

        private FortranSymbol GetColonSymbol(string value)
        {
            if (value == "::")
            {

                return new FortranSymbol(FortranSymbolType.Colon, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }

        private FortranSymbol GetDataTypeSymbol(string value)
        {
            if (value == "real" || value == "complex" || value == "integer" || value == "logical" || value == "character")
            {

                return new FortranSymbol(FortranSymbolType.DataType, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }
        

        private FortranSymbol GetCloseBraceSymbol(string value)
        {
            if (value == ")")
            {

                return new FortranSymbol(FortranSymbolType.CloseBrace, value, _index);
            }
            else
            {
                return GetErrorSymbol(_index, value);
            }
        }

        private FortranSymbol GetErrorSymbol(int index, string value)
        {
            return new FortranSymbol(FortranSymbolType.Error, value, _index);
        }

        private FortranSymbol GetArgumentSymbol(string value)
        {
            int i = 0;
            bool not_digit = false;
            while (i < value.Length)
            {
                if (IsDigit(value[i]))
                {
                    i++;
                }
                else
                {
                    not_digit = true;
                    break;
                }
            }

            if (not_digit || value == String.Empty)
            {
                return GetErrorSymbol(_index, value);
            }
            else
            {

                return new FortranSymbol(FortranSymbolType.Argument, value, _index);
            }
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я');
        }
    }
}
