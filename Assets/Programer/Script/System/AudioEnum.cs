/// <summary>BGMのタイプ</summary>
public enum BGMState
{
    /// <summary>タイトル</summary>
    Title,
    /// <summary>チュートリアル</summary>
    Tutorial,
    /// <summary>バトル</summary>
    Battle,
    /// <summary>リザルト</summary>
    Result,
}
/// <summary>SEのタイプ</summary>
public enum SEState
{
    /// <summary>Player通常攻撃発射音(氷パターン１)</summary>
    PlayerShootIce,
    /// <summary>Player通常攻撃発射音(氷パターン２)</summary>
    PlayerShootIce2,
    /// <summary>Player通常攻撃発射音(草パターン１)</summary>
    PlayerShootGrass,
    /// <summary>Player通常攻撃発射音(草パターン２)</summary>
    PlayerShootGrass2,
    /// <summary>Player必殺技チャージ音(氷パターン１)</summary>
    PlayerChargeIce,
    /// <summary>Player必殺技チャージ音(草パターン１)</summary>
    PlayerChargeGrass,
    /// <summary>Playerトレイル音(氷パターン1)</summary>
    PlayerTrailIcePatternA,
    /// <summary>Playerトレイル音(氷パターン2)</summary>
    PlayerTrailIcePatternB,
    /// <summary>Playerトレイル音(草パターン1)</summary>
    PlayerTrailGrassPatternA,
    /// <summary>Playerトレイル音(草パターン2)</summary>
    PlayerTrailGrassPatternB,
    /// <summary>Player魔法陣展開音(氷)</summary>
    PlayerMagiccirclIce,
    /// <summary>Player魔法陣展開中の音(氷)(弾が発射し終わった時にStop)</summary>
    PlayerMagiccirclPlayingIce,
    /// <summary>Player魔法陣展開音(草)</summary>
    PlayerMagiccirclGrass,
    /// <summary>Player魔法陣展開中の音(草)(弾が発射し終わった時にStop)</summary>
    PlayerMagiccirclPlayingGrass,
    /// <summary>Player回避音(氷)</summary>
    PlayerDodgeIce,
    /// <summary>Player回避音(草)</summary>
    PlayerDodgeGrass,
    /// <summary>Player遠距離敵からのダメージ</summary>
    PlayerLongAttackEnemyDamage,
    /// <summary>Playerボス敵からのダメージ(ボスが氷の場合)</summary>
    PlayerBossEnemyHitIce,
    /// <summary>Playerボス敵からのダメージ(ボスが草の場合)</summary>
    PlayerBossEnemyHitGrass,
    /// <summary>Playerの移動中の服の音</summary>
    PlayerClothMove,
    /// <summary>Playerの攻撃時(腕をふる時)の服の音</summary>
    PlayerClothAttack,

    /// <summary>Player通常攻撃が当たった時のHit音(氷パターン1)</summary>
    EnemyHitIcePatternA,
    /// <summary>Player通常攻撃が当たった時のHit音(氷パターン2)</summary>
    EnemyHitIcePatternB,
    /// <summary>Player通常攻撃が当たった時のHit音(草パターン1)</summary>
    EnemyHitGrassPatternA,
    /// <summary>Player通常攻撃が当たった時のHit音(草パターン2)</summary>
    EnemyHitGrassPatternB,
    /// <summary>Player必殺攻撃が当たった時のHit音(氷)</summary>
    EnemyFinichHitIce,
    /// <summary>Player必殺攻撃が当たった時のHit音(草)</summary>
    EnemyFinishHitGrass,
    /// <summary>近敵の攻撃音</summary>
    EnemyCloseAttack,
    /// <summary>遠敵の攻撃発射音</summary>
    EnemyLongAttackShoot,
    /// <summary>遠敵の攻撃のトレイル音</summary>
    EnemyLongAttackTrail,
    /// <summary>ボス敵の通常攻撃(氷)</summary>
    EnemyBossShootIce,
    /// <summary>ボス敵の通常攻撃(草)</summary>
    EnemyBossShootGrass,
    /// <summary>ボス敵の強攻撃チャージ中(氷)</summary>
    EnemyBossChargeIce,
    /// <summary>ボス敵の強攻撃チャージ中(草)</summary>
    EnemyBossChargeGrass,
    /// <summary>ボス敵のトレイル音(氷)</summary>
    EnemyBossTrailIce,
    /// <summary>ボス敵のトレイル音(草)</summary>
    EnemyBossTrailGrass,
    /// <summary>ボス敵の強攻撃(氷)</summary>
    EnemyBossFinishIce,
    /// <summary>ボス敵の強攻撃(草)</summary>
    EnemyBossFinishGrass,
    /// <summary>Playerの通常攻撃時のダメージ音</summary>
    EnemyNormalDamage,
    /// <summary>Playerのトドメ攻撃時のダメージ音</summary>
    EnemyFinishDamage,
    /// <summary>スタン音</summary>
    EnemyStan,
    /// <summary>登場音</summary>
    EnemyIn,
    /// <summary>退場音</summary>
    EnemyOut,
    
    /// <summary>システムの決定音</summary>
    SystemApply,
    /// <summary>システムのキャンセル音</summary>
    SystemCancel,
    /// <summary>システムの選択時の音(OptionのBGMやSEを選んでるとき等)</summary>
    SystemSelect,

    /// <summary>リザルトの成績発表に使う音(ドラムロール的な)</summary>
    MeScoreAnnouncement,
    /// <summary>リザルトの発表後に流れる音(評価が低い時)</summary>
    MeResultScoreLow,
    /// <summary>リザルトの発表後に流れる音(評価が普通の時)</summary>
    MeResultScoreMiddle,
    /// <summary>リザルトの発表後に流れる音(評価が高い時)</summary>
    MeResultScoreHigh,
}
/// <summary>Voiceのタイプ</summary>
public enum VoiceState
{
    /// <summary>通常攻撃(はっ！)(ふっ！)(やっ！)(これで！)</summary>
    PlayerAttack,
    /// <summary>氷魔法陣展開(グレイシオ！)(まだまだ！)(次・・・！)</summary>
    PlayerCastingIce,
    /// <summary>草魔法陣展開(ロゼエラ！)(まだまだ！)(次・・・！)</summary>
    PlayerCastingGrass,
    /// <summary>氷魔法チャージ中(冷気よ・・・！)(氷よ・・・！)(凍れ・・・！)</summary>
    PlayerChargeIce,
    /// <summary>草魔法チャージ中(薔薇よ・・・！)(緑よ・・・！)(命よ・・・！)</summary>
    PlayerChargeGrass,
    /// <summary>氷魔法必殺攻撃(熱を奪う！)(トドメ！)(これで終わり！)(コアを！)</summary>
    PlayerFinishIce,
    /// <summary>草魔法必殺攻撃(芽吹け・・・！)(トドメ！)(これで終わり！)(コアを！)</summary>
    PlayerFinishGrass,
    /// <summary>属性変更草から氷に変更するとき(氷で！)(氷魔法！)(こっちを！)</summary>
    PlayerAttributeChangeIce,
    /// <summary>属性変更氷から草に変更するとき(草で！)(草魔法！)(こっちを！)</summary>
    PlayerAttributeChangeGrass,
    /// <summary>回避(せっ！)(っと！)(っしょ！)</summary>
    PlayerDodge,
    /// <summary>ダメージ(うっ！)(痛った！)(ぐっ！)</summary>
    PlayerDamage,
    /// <summary>HP0(こんな・・・ところで・・・)(まだ・・・戦える・・・)(悔しい！)</summary>
    PlayerDown,

    /// <summary>近距離攻撃の敵索敵(オオオオォォ・・・・)</summary>
    EnemyShortSaerch,
    /// <summary>遠距離攻撃の敵索敵(オオオオォォ・・・・)</summary>
    EnemyLongSaerch,
    /// <summary>近距離攻撃の敵Player発見(キッ！)(ヒヒヒ！)</summary>
    EnemyShortDiscov,
    /// <summary>遠距離攻撃の敵Player発見(キッ！)(ヒヒヒ！)</summary>
    EnemyLongDiscov,
    /// <summary>近距離攻撃の攻撃(キヒャ！)(ケケケケ！)</summary>
    EnemyShortAttack,
    /// <summary>遠距離攻撃の攻撃(キヒャ！)(ケケケケ！)</summary>
    EnemyLongAttack,
    /// <summary>近距離攻撃のダメージ(キーッ!?)(ギ!?)</summary>
    EnemyShortDamage,
    /// <summary>遠距離攻撃のダメージ(キーッ!?)(ギ!?)</summary>
    EnemyLongDamage,
    /// <summary>近距離攻撃の死(ギーッ!)(ヒヒャーッ!)</summary>
    EnemyShortDeath,
    /// <summary>遠距離攻撃の死(ギーッ!)(ヒヒャーッ!)</summary>
    EnemyLongDeath,

    /// <summary>チュートリアル開始時
    /// (これより、アルシオネ魔法学園の、進級試験を始めます。
    /// 試験官は私、ミリアが担当するわね。
    /// 最初はノエリアさんの出番ね、頑張ってちょうだい。)</summary>
    InstructorTutorialStart,
    /// <summary>チュートリアル確認(貴方はとってもイイ感じなんだけど、不安だったらおさらいしておく?)</summary>
    InstructorTutorialCheck,
    /// <summary>チュートリアル確認後(分かったわ。それじゃあまずは)</summary>
    InstructorTutorialOK,
    /// <summary>移動チュートリアル開始(あの辺まで歩いてみてちょうだい)</summary>
    InstructorTutorialMove,
    /// <summary>移動チュートリアル完了(OKよ、次行くわね)</summary>
    InstructorTutorialMoveOK,
    /// <summary>カメラチュートリアル開始(周りを見回してみてちょうだい)</summary>
    InstructorTutorialCamera,
    /// <summary>カメラチュートリアル完了(いい調子よ、このままどんどん行きましょう)</summary>
    InstructorTutorialCameraOK,
    /// <summary>通常攻撃チュートリアル開始(お次は魔法を出してみてちょうだい、えいっと、ね)</summary>
    InstructorTutorialAttack,
    /// <summary>通常攻撃チュートリアル完了(カンタンだったかしら？それじゃあ次よ！)</summary>
    InstructorTutorialAttackOK,
    /// <summary>溜め攻撃チュートリアル開始(沢山1度に攻撃したかったらぐっと溜めてみるといいわよ)</summary>
    InstructorTutorialCharge,
    /// <summary>溜め攻撃チュートリアル完了(凄いわ！バッチリ！)</summary>
    InstructorTutorialChargeOK,
    /// <summary>必殺攻撃チュートリアル開始(敵を攻撃し続けるとトドメを刺せるのよ、もう知ってたかしら？)</summary>
    InstructorTutorialFinish,
    /// <summary>必殺攻撃チュートリアル完了(流石優等生ね！あと少しで先生のお話も終わりよ～)</summary>
    InstructorTutorialFinishOK,
    /// <summary>回避チュートリアル開始(危ない！と思ったら回避を使うのよ！)</summary>
    InstructorTutorialDodge,
    /// <summary>回避チュートリアル完了(うんうん、お見事！私が教えるまでもないわね！)</summary>
    InstructorTutorialDodgeOK,
    /// <summary>オプションチュートリアル開始(そうそう、何か困ったことあったら試験は一旦中断出来るわよ！startボタン・・・？っていうのを押せば良いわ～)</summary>
    InstructorTutorialOption,
    /// <summary>オプションチュートリアル完了(音が大きすぎる～って時は利用して頂戴ね！)</summary>
    InstructorTutorialOptionOK,
    /// <summary>ゲームクリア
    /// (は～い、試験終了ようふふっ、ノエリアさん、お疲れ様〜)
    /// (そこまで！よく頑張ったわね〜後で先生から合否を送るわね〜)
    /// (終了よ！今すぐ結果を報告したいけど、ちょっと待っててね〜)</summary>
    InstructorGameClear,
    /// <summary>回復
    /// (あらあら、調子が悪かったのかしら？まだまだ頑張れるわ！ファイト〜！)
    /// (大丈夫？先生の特別サ-ビスよ〜)
    /// (しっかり！貴方の実力はこんなものじゃないわ～！)</summary>
    InstructorGameHeal,
}