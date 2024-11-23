import React from 'react';
import { useState, useCallback } from 'react';
import { useHttp } from '../hooks/http-hook';
import ItemCard from '../Component/PageComponents/Scanner/ItemCard';
import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import { useTranslation } from "react-i18next";


function filterResults (results) {
    let filteredResults = [];
    for (var i = 0; i < results.length; ++i) {
        filteredResults.push(results[i])

        // if (i === 0) {
        //     filteredResults.push(results[i]);
        //     continue;
        // }

        // if (results[i].decodedText !== results[i - 1].decodedText) {
        //     filteredResults.push(results[i]);
        // }
    }
    return filteredResults;
}

function mode(array)
{
    if(array.length == 0)
        return null;
    var modeMap = {};
    var maxEl = array[0], maxCount = 1;
    for(var i = 0; i < array.length; i++)
    {
        var el = array[i];
        if(modeMap[el] == null)
            modeMap[el] = 1;
        else
            modeMap[el]++;  
        if(modeMap[el] > maxCount)
        {
            maxEl = el;
            maxCount = modeMap[el];
        }
    }
    return maxEl;
}

const ResultContainerTable = ({ data }) => {
    const {t} = useTranslation()
    const results = filterResults(data);
    return (
        <table className='qrcode-result-table'>
            <thead>
                <tr>
                    <th>#</th>
                    <th>{t('Decoded Text')}</th>
                    <th>{t('Format')}</th>
                </tr>
            </thead>
            <tbody>
                {
                    results.map((result, i) => {
                        return (
                            <tr key={i}>
                                <td>{i + 1}</td>
                                <td>{result.decodedText}</td>
                                <td>{result.result.format.formatName}</td>
                            </tr>
                        );
                    })
                }
            </tbody>
        </table>
    );
};

const ResultContainerPlugin = (props) => {
    const {t} = useTranslation()
    const {request} = useHttp()
    const {token} = useContext(AuthContext)
    let finalResult = ''
    let results = filterResults(props.results);
    let filteredText = [];
    results.map(x => filteredText.push(x.decodedText))
    const [data, setData] = useState([])
    let showItem = false
   
    if(filteredText.length === 10) {
        finalResult = mode(filteredText)
        showItem= true
        console.log(finalResult);
        // const fetchItem = async () => {
        //     try {
        //         const fetched = await request('/api/item/bynumber/'+finalResult, 'GET', null,  {'Authorization': 'Bearer ' + token})
        //         setData(fetched)
                
        //     } catch(e){}
        // }

        // await fetchItem()
    }

    

    return (
        <div className='result-container'>
            <div className='result-header'>{t('Scanned results:')} </div>
            <div className='result-section'>
                {filteredText.length < 10 ? <div>{t('Scanning...')}</div> : <div></div>}
            </div>
            <button className='clear-button' onClick={()=> {
                props.clearResult()
                finalResult = ''
                filteredText = []
                results = []
                showItem = false
            }}>{t('Clear')}</button>
    
            {showItem && <ItemCard number={finalResult}/>} 
        </div>
    );
};

export default ResultContainerPlugin;