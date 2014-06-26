markhovGenerator
================

Creates a word generator based off of occurance of letter sequences in a given list of words.

the markhov Generator reads in a dict.txt file (in the same directory as the .exe) and compiles an array of markhov chain pieces.
I have used a simple dict.txt like:
https://raw.githubusercontent.com/eneko/data-repository/master/data/words.txt

Features:
 * Include directives (only words with specified letter combinations are allowed to be generated)
 * Exclude directives (only words without specified letter combinations are allowed to be generated)
 * chain viewing. (type in a 2 letter link and it will display all corresponding links based on the loaded dict.txt)
 * save generated words. (gives the option to append generated words into a file called 'output.txt')

