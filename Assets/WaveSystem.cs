using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class WaveSystem : MonoBehaviour
{
    [Header("Wave")]
    //public List<WaveData> waveDatas = new List<WaveData>();
    public WaveData waveData;
    public int currentWave = -1; //currentWave == waveData => List<SpawnTimeScript> Index
    public float currentTime;
    //public float startGameTime; //or 게임 시작전 런타임 시간 0

    private void Awake()
    {
        InitalizeData();
    }
    void InitalizeData()
    {
        //waveData 정렬
        waveData.SortScriptsbyTime();

        if (waveData.TimeScripts.Count > 0 || waveData.TimeScripts != null)
        {
            currentWave = 0;
        }
    }

    private void Update()
    {
        currentTime = Time.timeSinceLevelLoad; //씬이 로드된 이후의 시간 기준
        StartWaveProgress();
    }

    //Wave Process
    //SpawnEnemies
    //End Spawn
    //EndWave
    //Clear Or Fail Event

    private void StartWaveProgress()
    {
        //런타임 시간 기준으로 wave 변형, 런타임 시간과 현재 웨이브의 스폰 시작시간 비교
        if (waveData.TimeScripts.Count <= 0 || waveData.TimeScripts == null)
        {
            Debug.LogWarning($"[WaveSystem] waveData의 개수: {waveData.TimeScripts.Count} / StartWaveProgress() 실행 실패");
            return;
        }

        if (currentWave >= waveData.TimeScripts.Count || currentWave < 0)
        {
            Debug.LogWarning($"[WaveSystem] 현재 웨이브: {currentWave} / 현재 웨이브는 waveData에 존재하지 않습니다.");
            Debug.LogWarning($"[WaveSystem] 현재 웨이브: {currentWave} / 웨이브를 종료합니다.");
            EndWave();
            return;
        }

        //현재 웨이브 스폰 시작 시간과 현재 시간사이가 0.01초 라면 스폰 시작
        if (Mathf.Abs(waveData.TimeScripts[currentWave].startTime - currentTime) <= 0.001f)
        {
            if (waveData.TimeScripts[currentWave].isBoss)
            {
                Debug.Log($"[WaveSystem] 보스 출현 웨이브");
            }
            SpawnEnemy();
            Debug.Log($"[WaveSystem] Time: {currentTime} / 현재 웨이브: {currentWave} / 적 스폰 생성이 끝나 다음 웨이브 준비");
            currentWave++;
        }
    }

    public void SpawnEnemy()
    {
        //생성될 스폰 그룹의 개수 만큼 실행
        for (int i = 0; i < waveData.TimeScripts[currentWave].spawnGroups.Count; i++)
        {
            StartCoroutine(SpawnProgress(waveData.TimeScripts[currentWave].spawnGroups[i].Interval, i));
        }
    }

    public void EndWave()
    {
        //GameManager 혹은 Boss Enemy과 연동
        Debug.Log("[WaveSystem] 웨이브가 끝났습니다. / 데이터를 초기화 합니다.");
        InitalizeData();
    }

    IEnumerator SpawnProgress(float delay, int index)
    {
        foreach (SpawnObj obj in waveData.TimeScripts[currentWave].spawnGroups[index].spawnObjs)
        {
            yield return new WaitForSeconds(delay);
            GameObject instance = ObjectPoolManager.instance.GetPool(obj.poolID, new Vector3(0, 16, 0), Quaternion.identity);
            if (instance.TryGetComponent<Enemy>(out Enemy enemy))
            {
                    enemy.pathID = obj.pathID;
            }
        }
        
    }
}
