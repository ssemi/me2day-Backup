#!/usr/bin/env python
#-*- coding: utf-8 -*-

from me2lib import *

me2bb = me2Backup()
me2bb.timesleep = 1.0;
me2bb.dataFileType = "xml"
me2bb.userid = ("ssemi",)
me2bb.crawlingLastestDate = "2014-06-20"
#me2bb.saveUserData('ssemi', '2014-06-30')
me2bb.saveUserDataCrawler()
