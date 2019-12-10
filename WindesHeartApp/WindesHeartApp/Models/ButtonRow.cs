using System.Collections.Generic;
using Xamarin.Forms;

namespace WindesHeartApp.Models
{
    public class ButtonRow
    {

        private List<Button> _buttons;
        private int _selectedIndex;

        public ButtonRow(List<Button> buttons)
        {
            _buttons = buttons;
            _selectedIndex = buttons.Count - 1;
        }

        public bool ToNext()
        {
            //If there is another button left
            if (_selectedIndex + 1 < _buttons.Count)
            {
                //If there was an old button
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = true; //enable old button
                }

                _selectedIndex++;

                //If there is a new button
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = false; //disable new button
                }
                return true;
            }
            return false;
        }

        public bool ToPrevious()
        {
            //If old index is 0 or above
            if (_selectedIndex >= 0)
            {
                _buttons[_selectedIndex].IsEnabled = true; //enable old button
                _selectedIndex--;

                //If new index is 0 or above
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = false; //disable new button
                }
                return true;
            }
            else
            {
                _selectedIndex--;
                return false;
            }
        }

        public bool SwitchTo(Button b)
        {
            //If that button exists, get the index
            int index = _buttons.FindIndex(button => button.Equals(b));
            if (index >= 0)
            {
                if (_selectedIndex >= 0)
                {
                    _buttons[_selectedIndex].IsEnabled = true; //enable old button
                }

                _selectedIndex = index;
                _buttons[_selectedIndex].IsEnabled = false; //disable new button
                return true;
            }
            return false;
        }
    }
}
