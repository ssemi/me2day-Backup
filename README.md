me2day-Backup
=============
# 사용 목적
개인적으로는 미투데이의 글들이 소중하다고 생각되어졌는데, 
저장을 하고 싶은데 백업은 지원안해주는 미투데이.
그래서 만들었음둥.

# 실행 환경
.NET Framework 4.5가 설치된 Windows OS상에서 실행
.NET Framework 4.5 Download 
- http://www.microsoft.com/en-us/download/details.aspx?id=30653

# 백업 해주는 것들
- 자신의 글
- 자신의 글에 달린 댓글
- 자신이 올린 me2photo 사진 
(주의 : 2장 이상이라면 첫번째로 올린 사진만 다운됨)

# 백업 안해줌
- 친구 관련
- 미투 관련
- 다른 사람들 관련
- 여러장 me2photo사진
- 쪽지
- 동영상
- 보이스
- 글감
- 위치
- 긴 글 노트 
$$$ 위에 말들 필요없고 API에서 지원 안하는건 못 하고, 나머지는 귀찮아서가 정답. $$$

# 사용방법
1. 미투데이 로그인
2. 백업 시작
3. 상태창에 - 끝 - 뜨면 끝임.

# 에러 상황
2-1. 간혹 아래 에러를 만남
Error Descriotion : Connection reset
Error Code : 1027
Error Message : 요청 에러가 발생하였습니다.
- data 폴더에 가서 몇일꺼까지 백업이 되었나 확인해봄 
- 해당 일을 [기준 날짜]에 맞춰서 다시 2. 백업 시작

2-2. 간혹 코멘트 부분이 제대로 백업이 안되어 있을 수도 있음. 
Error Code : 1001
Error Message : 권한이 없습니다.
- 해당 일을 [기준 날짜]에 맞춰서 다시 2. 백업 시작

3-1. output 폴더에 가면 보기 좋게 정리해 놨음.
궁금하거나 기타 에러 사항 있으면, 아래에서 가장 최근 글에다가 남겨주면 됨.


Contact URL : http://me2day.net/ssemi/tag/me2backup



# P.S
코멘트가 1,000개 넘는 사람 없겠지??? - 제한 걸어뒀는데…
미투데이 오래한 혹은 글 많이 쓴 사람들은 1년 단위로 나눠서 받는게 정신건강(?)에 이로움. 
본인이 해봤다가 피봤음 -_-;;;

# 개발자님들을 위한 FAQ
아마도 해당 Application Key에 대한 제한이 있을 것으로 생각됨으로 인하여
해당 실행파일내에 Appkey.txt 를 만들어서 본인 Application Key 넣고 돌리면 원할?하게 돌릴 수 있음.

하루에 글이 많으면 코멘트도 많아져서 에러가 많아지는데 그럼에도 불구하고 글과 코멘트를 전부다 잘 얻고 싶다면..
날짜 조절 잘해서 계속 data 파일 정확하게 얻으면 됨.

예를 들어 1년짜리로 미리 제한 걸어서 
로그인 후에 미투 가입일을 2013-01-01로 맞추고 2013년도를 뽑아냄.
같은 방식으로 2012, 2011년도를 Data를 뽑아내고 이후에 HTML 병합이 이루어지게 해주면 됩니다.

여러 사용자가 미투데이를 각자의 방식으로 쓰고 있기 때문에 제가 생각지도 못한 에러들이 있을 수도 있어요.
에러는 댓글로 이만 총총 =3=3=3

# 미리보기
https://dl.dropboxusercontent.com/u/3748960/ssemi/html/me2day_ssemi_2013.html

# 폴더구조
https://dl.dropboxusercontent.com/u/3748960/me2backup-folder.png

# 다운로드
https://dl.dropboxusercontent.com/u/3748960/me2backup.zip
