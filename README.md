# TaskSwitcher
TaskSwitcher is a simple program designed to give the focus to an already-opened window, based on window title.

Alternatively, if no window matches that title, it can launch an executable with a specific set of arguments.

It was originally written to solve a change in behavior with Windows 10 (or possibly PuTTY), such that launching a program via shortcut (including hotkey), would cause a new version of the window to open, instead of switching to the already-opened window. This behavior can be seen in programs like PuTTY and MsPaint, but other programs like Notepad will still switch to the already-opened window (I'm not sure why). The problem is described in more detail here:

https://superuser.com/questions/1208543/change-in-windows-10-shortcut-key-behavior

## Usage

You can simply use TaskSwitcher on the Windows command line, but its true power lies using it as a shortcut. To create a shortcut to reopen the same window (or a new one, if no window title matches) create a normal Windows shortcut, with the "target" being something like the following example:

C:\TaskSwitcher-v1.0\TaskSwitcher.exe "PuTTY-WindowTitle" "C:\Program Files\PuTTY\putty.exe" "-load MyPuTTYSession"

You can then create a shortcut hotkey (e.g. CTRL-ALT-1, CTRL-ALT-2, etc) to switch to that window, or launch if it isn't opened.

You will generally want to put your shortcuts in a folder such as the following:

C:\Users\UserName\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\_hotkeys

### Listing windows

If you are having trouble figuring out the correct window title, use the "list" argument to see all of the window titles that are currently opened.

C:\TaskSwitcher-v1.0\TaskSwitcher.exe list
PuTTY-WindowTitle1
PuTTY-WindowTitle2
etc

Beware of any trailing spaces.

## Known issues / Future improvements

There are some known issues, for example, it requires an unchanging window title. I use GNU Screen in my PuTTY sessions, so this isn't a problem, and it's easy to customize each screen window title. I may fix this in the future with either partial title-matches, or regular expressions. 

### 3-second Pause 

Normally, switching to the opened window should be nearly instant. Occasionally, there is a pause of about 3 seconds. Unfortunately, once the 3 second delay starts, it persists. There are a few different suggestions to fix this online, including disabling Superfetch. Odd as it sounds, What I've found works for me is opening your Privacy settings, toggling the settings you have off/on, then closing the window.

For more info: https://superuser.com/questions/426947/slow-windows-desktop-keyboard-shortcuts

### Window state

The program will restore the window to its original state. I usually keep my windows maximized, which is fine, but I would like to add the ability to choose the window state (e.g. always start maximized).   
