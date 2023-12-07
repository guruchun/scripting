function main(workbook: ExcelScript.Workbook) {
    //Application.EnableEvents = False;
    // sheet format -----------------------
    //  Row 0: old tag name
    //  Row 1: select column for generating test data
    //  Row 2: new tag name (updated by script)
    //  Row 3: check changed condition (delta of between previous and current value)
    //  Row 4 ~ : data
    // ------------------------------------
  
    let actSheet = workbook.getActiveWorksheet();
    let activeCell = workbook.getActiveCell();
    let currColIdx = activeCell.getColumnIndex();
    let lastRowIdx = actSheet.getUsedRange().getLastRow().getRowIndex();
    let lastColIdx = actSheet.getUsedRange().getLastColumn().getColumnIndex();
    console.log("row=4" + "~" + lastRowIdx + ", col=" + currColIdx + "~" + lastColIdx);
    if (lastRowIdx < 4) {
      return;
    }
  
    let rangeValues = actSheet.getUsedRange().getValues();
    while (currColIdx <= lastColIdx) {
      let currRowIdx = 4;
      let delta = rangeValues[3][currColIdx]; // from row 'delta' = 3
      if (delta == NaN || delta < 0) {
        delta = 0.1       // default delta=0.1
      }
      console.log("col=" + currColIdx + ", delta=" + delta
        + ", old=" + rangeValues[0][currColIdx] + ", new=" + rangeValues[1][currColIdx]);
  
      let prevValue = 0;
      let currValue = 0;
      while (currRowIdx < lastRowIdx) {
        currValue = rangeValues[currRowIdx + 1][currColIdx] as number;
        if (Math.abs((prevValue - currValue)) > delta) {
          actSheet.getCell(currRowIdx + 1, currColIdx).getFormat().getFill().setColor("yellow");
          prevValue = currValue;
        } else {
          actSheet.getCell(currRowIdx + 1, currColIdx).getFormat().getFill().clear();
        }
        // go next row
        currRowIdx++;
      }
      // go next column with row index reset
      currColIdx++;
    }
  
    console.log("finished...");
    //Application.EnableEvents = True
  }
  