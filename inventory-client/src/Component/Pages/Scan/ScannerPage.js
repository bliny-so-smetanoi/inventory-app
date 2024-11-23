import { useEffect, useState } from "react"
import Html5QrcodePlugin from "../../../plugin/ScannerPlugin"
import ResultContainerPlugin from "../../../plugin/FilterPlugin";
import  *  as rdd from "react-device-detect";

export default function ScannerPage() {
    const [decodedResults, setDecodedResults] = useState([]);
    const onNewScanResult = (decodedText, decodedResult) => {
        setDecodedResults(prev => [...prev, decodedResult]);
    }
//{width: 195, height: 56} rdd.isMobile ? 0.4 : 1.3
    return (
        <>
        <div className="scanner-page-wrapper">
        <div className='scanner-page-container'>
            
            {decodedResults.length < 10 && <Html5QrcodePlugin
            fps={20}
            qrbox={!rdd.isMobile ? {width: 400, height: 300} : {width: 230, height: 70}}
            disableFlip={false}
            qrCodeSuccessCallback={onNewScanResult}
            aspectRatio={rdd.isMobile ? 2.8 : 1.33}
            />}
            
        </div>
        
            <ResultContainerPlugin clearResult={() => setDecodedResults([])} results={decodedResults}/>
            
        </div>
        </>
    )
}