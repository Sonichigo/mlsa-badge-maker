import React, { useState } from 'react';
import { useAppDispatch } from '../../app/hooks';

const Editor = () => {
  const dispatch = useAppDispatch();
  const [file, setFile] = useState<File>();

  // read browser file from input
  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let files = e.currentTarget.files;
    if (files?.item(0))
      setFile(files[0]);

    if (file) {
      dispatch({
        type: 'EDITOR_FILE_CHANGE',
        payload: {
          file
        }
      });
    }
  };

  return (
    <div className="editor">
      <input
        type="file"
        accept="image/*"
        onChange={onFileChange}
        style={{ display: 'none' }}
      />
      <div className="editor-container">
        {/* {file && (
          <img src={file} />
        )} */}
      </div>
    </div>
  );
};

export default Editor;