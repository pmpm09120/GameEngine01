using UnityEngine;

public class LevelScriptData : MonoBehaviour
{
    ///<summary>
    ///Spawn System -> 타임라인 및 웨이브 기반 생성
    ///생성 데이터 구조] 예) 등장 시간, 적ID, 생성 위치(x, y), 시작 방향---
    ///스테이지 진행표(웨이브 시나리오) 데이터 => 생성 시점, 생성 위치, 생성 객체, 지정 경로
    ///
    /// 
    /// 웨이브 시스템 > 웨이브 카운트 및 생성 시스템 호출
    /// 생성 시스템> 생성 시점, 생성 주기, 생산
    /// 레벨 스크립트 > 웨이브, 생성 시점 or 조건, 적 생성량, 생성 지연, 생성될 적(초기위치, 이동패턴id, 적 종류)
    ///<summary>
    /*
     * SpawnGroup
     * [
     * Enemy Type,
     * Amount,
     * spawn_Interval
     * ]
     * ***
     * SpawnTimeScript
     * [
     * start_time,
     * List<SpawnGroup>
     * ]

***
waveData
[
wave_id,
List<SpawnTimeScript>
]

***
List<waveData>

생성 메서드 복수 실행 가능. ? or 순차실행 ?
타임라인 스크립트 시간순서 기준으로 리스트 정렬.


    EX)
    waves(id: 901) -> Boss(id: 902) -> waves(id: 903)...
    wave 데이터가 분리되어 시간 순서 작성에서 오차가 발생할 수 있음

     EX_2)
    단일 웨이브 데이터
    시간 순서 작성에서의 오차가 줄어듦, 단 시간 순서로만 웨이브가 진행되기에 특수 조건에 대한 조건부 처리가 필요

    EX_3)
    Wave(일반 웨이브, 보스 웨이브)
    보스 웨이브가 끝나면 새 웨이브 실행
     */


}
