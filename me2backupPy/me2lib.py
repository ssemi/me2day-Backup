#-*- coding: utf-8 -*-
import os, sys
reload(sys)
sys.setdefaultencoding('utf-8')
import urllib, urllib2
from datetime import date, timedelta, datetime
import json, urlparse, time
import xml.etree.ElementTree as ET
from multiprocessing import Process, Queue
from Queue import Empty

class me2Backup(object):

	def __init__(self):
		self.dataFolder = "me2userdata"
		self.outputFolder = "useroutput"
		self.dataFileType = "json"
		self.timesleep = 10.0
		self.crawlingLastestDate = None
		self.userid = ""

		if not os.path.exists(self.dataFolder):
			os.makedirs(self.dataFolder)
			
	def getUsers(self):
		return self.userid

	def saveUserDataLastest(self):
		users = self.getUsers()
		for i in users:
			self.saveUserData(i) 
	
	def saveUserData(self, userid):
		dfrom = (date.today() - timedelta(days=1))
		self.saveUserData(userid, dfrom)

	def saveUserData(self, userid, nstart=date.today()):
		if type(nstart) == str:
			nstart = datetime.strptime(nstart, "%Y-%m-%d").date()

		now = nstart
		dfrom = now.strftime("%Y%m%d") 
		dto = (now + timedelta(days=1)).strftime("%Y%m%d")

		self.saveMe2dayUserData(userid, dfrom, dto, self.dataFileType)

	def saveMe2dayUserData(self, userid, nfrom, nto, ntype='json'):
		if not os.path.exists(self.dataFolder + '/' + userid):
			os.makedirs(self.dataFolder + '/' + userid)

		try:
			url = "http://me2day.net/api/get_posts/%s.%s?from=%s&to=%s" % (userid, ntype, nfrom, nto,)
			request = urllib2.Request(url)
			response = urllib2.urlopen(request)
			me2dailyjson = response.read()
		except:
			me2dailyjson = ''

		if "post_id" in me2dailyjson:
			self.saveMe2dayUserDataCommentMulti(me2dailyjson, userid, nfrom, self.dataFileType)
		
			saveFileName = "%s/%s/%s_%s.%s" % (self.dataFolder, userid, userid, nfrom, ntype,)
			open(saveFileName, "w").write(me2dailyjson)
			print '%s - %s' % (userid, nfrom)
	
	def saveMe2dayUserDataCommentMulti(self, dailydata, userid, ndate, ntype='json'):
		j = json.loads(dailydata) if ntype == 'json' else ET.fromstring(dailydata)

		try:
			work_queue = Queue()
			for i in range(1,50):
				work_queue.put(i)

			processes = [Process(target=userDataCommentCrawlingWork, args=(work_queue, (e['post_id'] if ntype=='json' else e[0].text), self.dataFolder, userid, ndate, ntype, )) for e in j]

			for p in processes:
				p.start()

			for p in processes:	
				p.join()	

		except:
			pass

	def saveMe2dayUserDataComment(self, dailydata, userid, ndate, ntype='json'):
		j = json.loads(dailydata) if ntype == 'json' else ET.fromstring(dailydata)

		try:
			for e in j:
				post_id = e['post_id'] if ntype=='json' else e[0].text
				try:
					url = "http://me2day.net/api/get_comments/%s.%s?post_id=%s&items_per_page=1000" % (userid, ntype, post_id,)
					request = urllib2.Request(url)
					response = urllib2.urlopen(request)
					me2commentjson = response.read()
				except:
					me2commentjson = ''

				if len(me2commentjson) > 2:
					saveFileName = "%s/%s/%s_%s_%s.%s" % (self.dataFolder, userid, userid, ndate, post_id, ntype,)
					open(saveFileName, "w").write(me2commentjson)
		except:
			pass
	
	def saveUserDataCrawler(self):
		users = self.getUsers()

		work_queue = Queue()
		for i in range(1,50):
			work_queue.put(i)

		processes = [Process(target=userDataPostCrawlingWork, args=(work_queue, userid, self, )) for userid in users]

		for p in processes:
			p.start()

		for p in processes:	
			p.join()	

	def getRegisteredDate(self, userid):
		try:
			registDate = date.today()
			url = "http://me2day.net/api/get_person/%s.json" % userid
			request = urllib2.Request(url)
			response = urllib2.urlopen(request)
			me2personjson = response.read()

			if len(me2personjson) > 2:
				j = json.loads(me2personjson)
				if type(j) == dict:
					registDate = datetime.strptime(str(j['registered'])[:10], "%Y-%m-%d").date() 

			return registDate
		except:
			return date.today()


def userDataPostCrawlingWork(q, userid, objClass):
	registDate = objClass.getRegisteredDate(userid)
	if objClass.crawlingLastestDate is not None:
		registDate = datetime.strptime(objClass.crawlingLastestDate[:10], "%Y-%m-%d").date() 

	totalDays = (date.today() - registDate).days
	
	z = 0
	#while z<2:
	while z<totalDays:
		try:
			nstart = date.today()-timedelta(days=z)
			#print userid+'\t'+ str(nstart) +'\n'
			objClass.saveUserData(userid, nstart)
			z+=1
			time.sleep(objClass.timesleep)

		except Empty:
			pass

def userDataCommentCrawlingWork(q, post_id, dataFolder, userid, ndate, ntype='json'):
	try:
		url = "http://me2day.net/api/get_comments/%s.%s?post_id=%s&items_per_page=1000" % (userid, ntype, post_id,)
		request = urllib2.Request(url)
		response = urllib2.urlopen(request)
		me2commentjson = response.read()
	except:
		me2commentjson = ''

	if len(me2commentjson) > 2:
		saveFileName = "%s/%s/%s_%s_%s.%s" % (dataFolder, userid, userid, ndate, post_id, ntype,)
		open(saveFileName, "w").write(me2commentjson)	


#if __name__=="__main__":
	#me2bb = me2Backup()
	#me2bb.dataFileType = "xml"
	#me2bb.timesleep = 1.0;
	#me2bb.saveUserDataLastest()
	#me2bb.saveUserDataCrawler()
	#me2bb.saveUserData('zoro', '2013-07-18')
	
	