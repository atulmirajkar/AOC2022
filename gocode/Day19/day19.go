package main

import (
	"bufio"
	"fmt"
	"log"
	"math"
	"os"
	"strconv"
	"strings"
)

func readFile(filePath string) []string {
	file, err := os.Open(filePath)
	if err != nil {
		log.Fatal("failed to open")
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanLines)
	var lineArr []string

	for scanner.Scan() {
		lineArr = append(lineArr, scanner.Text())
	}
	return lineArr
}

func main() {
	lineArr := readFile("testdata.txt")
	var bpArr []BluePrint
	for _, line := range lineArr {
		lineInfo := strings.Split(line, ",")
		var bp BluePrint
		bp.id, _ = strconv.Atoi(lineInfo[0])
		bp.oreRobotOre, _ = strconv.Atoi(lineInfo[1])
		bp.clayRobotOre, _ = strconv.Atoi(lineInfo[2])
		bp.obsRobotOre, _ = strconv.Atoi(lineInfo[3])
		bp.obsRobotClay, _ = strconv.Atoi(lineInfo[4])
		bp.gdRobotOre, _ = strconv.Atoi(lineInfo[5])
		bp.gdRobotObs, _ = strconv.Atoi(lineInfo[6])

		bpArr = append(bpArr, bp)
	}

	fmt.Println(part1(bpArr))
}

func part1(bpArr []BluePrint) int {
	cnt := 0
	totalTime := 24

	for _, bp := range bpArr {
		resourceInfo := ResourceInfo{numOre: 0, numClay: 0, numObs: 0}
		robotInfo := RobotInfo{numOreRobot: 1, numClayRobot: 0, numObsRobot: 0, numGdRobot: 0}
		memo := make(map[string]int)
		cnt += (bp.id * evalBP(bp, resourceInfo, robotInfo, 1, totalTime, memo))
	}

	return cnt
}

/*
recursive function
evalBP(time, resources, robots)

	return numGeode with current resources
	+
	max
		evalBP(time+1, resources + robotProduction - resources for a robot, robot+somerobot)
	also
		evalBP(time+1, resources + robotProdcution, robot) //no new robot
*/
func evalBP(bp BluePrint, resourceInfo ResourceInfo, robotInfo RobotInfo, currTime int, totalTime int, memo map[string]int) int {
	if currTime > totalTime {
		return 0
	}
	// fmt.Println(currTime, resourceInfo, robotInfo)
	hash := getHash(currTime, resourceInfo, robotInfo)
	if v, ok := memo[hash]; ok {
		return v
	}

	//recurse
	maxGeode := math.MinInt
	newRes := updateResourceFromRobot(resourceInfo, robotInfo)

	//try creating ore robot
	if resourceInfo.numOre >= bp.oreRobotOre {
		// fmt.Println("creating ore robot")

		result := evalBP(bp, addResource(newRes, ResourceInfo{numOre: -1}), addRobot(robotInfo, RobotInfo{numOreRobot: 1}), currTime+1, totalTime, memo)
		maxGeode = getMaxInt(maxGeode, result)
	}
	//try creating clay robot
	if resourceInfo.numOre >= bp.clayRobotOre {
		// fmt.Println("creating clay robot")

		result := evalBP(bp, addResource(newRes, ResourceInfo{numOre: -1}), addRobot(robotInfo, RobotInfo{numClayRobot: 1}), currTime+1, totalTime, memo)
		maxGeode = getMaxInt(maxGeode, result)
	}

	//try creating clay robot
	if resourceInfo.numOre >= bp.obsRobotOre && resourceInfo.numClay >= bp.obsRobotClay {
		// fmt.Println("creating obs robot")

		result := evalBP(bp, addResource(newRes, ResourceInfo{numOre: -1, numClay: -1}), addRobot(robotInfo, RobotInfo{numObsRobot: 1}), currTime+1, totalTime, memo)
		maxGeode = getMaxInt(maxGeode, result)
	}

	//try creating geode robot
	if resourceInfo.numOre >= bp.gdRobotOre && resourceInfo.numObs >= bp.gdRobotObs {
		// fmt.Println("creating gd robot")

		result := evalBP(bp, addResource(newRes, ResourceInfo{numOre: -1, numObs: -1}), addRobot(robotInfo, RobotInfo{numGdRobot: 1}), currTime+1, totalTime, memo)
		maxGeode = getMaxInt(maxGeode, result)
	}

	//try without creating any robotInfo
	result := evalBP(bp, newRes, robotInfo, currTime+1, totalTime, memo)
	maxGeode = getMaxInt(maxGeode, result)

	if maxGeode == math.MinInt {
		maxGeode = 0
	}

	// fmt.Println(robotInfo.numGdRobot + maxGeode)
	memo[hash] = robotInfo.numGdRobot + maxGeode
	return memo[hash]
}

func getHash(time int, resInfo ResourceInfo, robotInfo RobotInfo) string {
	return strconv.Itoa(time) + ":" + resInfo.toString() + ":" + robotInfo.toString()
}

func getMaxInt(a, b int) int {
	result := math.Max(float64(a), float64(b))
	return int(result)
}

func updateResourceFromRobot(resourceInfo ResourceInfo, robotInfo RobotInfo) ResourceInfo {
	newNumOre := resourceInfo.numOre + robotInfo.numOreRobot
	newNumClay := resourceInfo.numClay + robotInfo.numClayRobot
	newNumObs := resourceInfo.numObs + robotInfo.numObsRobot

	return ResourceInfo{numOre: newNumOre, numClay: newNumClay, numObs: newNumObs}
}

func addResource(a, b ResourceInfo) ResourceInfo {
	return ResourceInfo{numOre: a.numOre + b.numOre, numClay: a.numClay + b.numClay, numObs: a.numObs + b.numObs}
}

func addRobot(a, b RobotInfo) RobotInfo {
	newNumOreRobot := a.numOreRobot + b.numOreRobot
	newNumClayRobot := a.numClayRobot + b.numClayRobot
	newNumObsRobot := a.numObsRobot + b.numObsRobot
	newNumGdRobot := a.numGdRobot + b.numGdRobot

	return RobotInfo{numOreRobot: newNumOreRobot, numClayRobot: newNumClayRobot, numObsRobot: newNumObsRobot, numGdRobot: newNumGdRobot}
}

type RobotInfo struct {
	numOreRobot  int
	numClayRobot int
	numObsRobot  int
	numGdRobot   int
}

func (r RobotInfo) toString() string {
	return strconv.Itoa(r.numOreRobot) + ":" + strconv.Itoa(r.numClayRobot) + ":" + strconv.Itoa(r.numObsRobot) + ":" + strconv.Itoa(r.numGdRobot)
}

type ResourceInfo struct {
	numOre  int
	numClay int
	numObs  int
}

func (r ResourceInfo) toString() string {
	return strconv.Itoa(r.numOre) + ":" + strconv.Itoa(r.numClay) + ":" + strconv.Itoa(r.numObs)
}

type BluePrint struct {
	id           int
	oreRobotOre  int
	clayRobotOre int
	obsRobotOre  int
	obsRobotClay int
	gdRobotOre   int
	gdRobotObs   int
}
